using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;
using Ofx.Battleships.Domain.DomainAggregates.GameAggregate.Exceptions;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate.Exceptions;

namespace Ofx.Battleships.Domain.DomainServices.GameService;

public interface IGameDomainService
{
    public Task<Game> CreateSinglePlayerGame(Guid playerId);
    public Task<Ship> AddBattleshipAsync(Guid gameId, Guid playerId, Point start, int length, LayoutDirection layoutDirection);
    public Task<Game> StartGame(Guid gameId);
    public Task<(bool isShipHit, bool isShipSunk)> AttackAndVerify(Guid gameId, Guid playerId, Point point);
}

public class GameDomainService : IGameDomainService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IGameRepository _gameRepository;

    public GameDomainService(IPlayerRepository playerRepository, IGameRepository gameRepository)
    {
        _playerRepository = playerRepository;
        _gameRepository = gameRepository;
    }
    
    public async Task<Game> CreateSinglePlayerGame(Guid playerId)
    {
        var player = await _playerRepository.GetPlayerAsync(playerId);
        if (player == null) throw new PlayerDoesNotExistException($"Invalid player ID {playerId}.");

        var computerPlayer = await _playerRepository.GetPlayerAsync("computer");
        if (computerPlayer == null) throw new PlayerDoesNotExistException($"Failed to initialize computer player.");
        
        var game = new Game
        {
            IsSinglePlayer = true
        };
        game.AddParticipantA(player);
        game.AddParticipantB(computerPlayer);

        InitComputerBattleships(game, computerPlayer);
            
        return await _gameRepository.AddAsync(game);
    }

    public async Task<Ship> AddBattleshipAsync(Guid gameId, Guid playerId, Point start, int length, LayoutDirection layoutDirection)
    {
        var game = await _gameRepository.GetGameAsync(gameId);
        if (game == null) throw new GameDoesNotExistException($"Invalid game ID {gameId}.");

        var ship = game.AddShip(playerId, start, length, layoutDirection);
        await _gameRepository.UpdateAsync(game);
        
        return ship;
    }

    public async Task<Game> StartGame(Guid gameId)
    {
        var game = await _gameRepository.GetGameAsync(gameId);
        if (game == null) throw new GameDoesNotExistException($"Invalid game ID {gameId}.");
        
        game.StartGame();
        return game;
    }

    public async Task<(bool isShipHit, bool isShipSunk)> AttackAndVerify(Guid gameId, Guid playerId, Point attackPoint)
    {
        var game = await _gameRepository.GetGameAsync(gameId);
        if (game == null) throw new GameDoesNotExistException($"Invalid game ID {gameId}.");

        if (game.PlayingParticipant.Player.Id != playerId)
            throw new GamePlayerTurnException(
                $"Invalid move. Currently playing player is {game.PlayingParticipant.Player.Id}");

        var opponentParticipant = game.ParticipantA.Player.Id == playerId ? game.ParticipantB : game.ParticipantA;

        var coordinate = opponentParticipant.Board.Coordinates.First(c => c.X == attackPoint.X && c.Y == attackPoint.Y);

        var beforeHitCoordinateShipStatus = coordinate.Ship != null && coordinate.Hit;
        var beforeHitShipSunkStatus = coordinate.Ship?.IsSunk;
        
        opponentParticipant.Board.Attack(attackPoint);

        var afterHitCoordinateShipStatus =  coordinate.Ship != null && coordinate.Hit;
        var afterHitShipSunkStatus = coordinate.Ship?.IsSunk;

        return (beforeHitCoordinateShipStatus == false && afterHitCoordinateShipStatus, beforeHitShipSunkStatus == false && afterHitShipSunkStatus == true);
    }
    
    private void InitComputerBattleships(Game game, Player player)
    {
        game.AddShip(player.Id, new Point{ X = 0, Y = 0}, 10, LayoutDirection.Horizontal);
        game.AddShip(player.Id, new Point{ X = 1, Y = 1}, 6, LayoutDirection.Horizontal);
        game.AddShip(player.Id, new Point{ X = 3, Y = 2}, 4, LayoutDirection.Horizontal);
        game.AddShip(player.Id, new Point{ X = 4, Y = 6}, 3, LayoutDirection.Horizontal);
        game.AddShip(player.Id, new Point{ X = 6, Y = 1}, 4, LayoutDirection.Horizontal);
        game.AddShip(player.Id, new Point{ X = 8, Y = 2}, 6, LayoutDirection.Horizontal);
    }

    private void SimulateComputerAttack(Game game, Player player)
    {
        
    }
}