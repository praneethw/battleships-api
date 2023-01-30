using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;

namespace Ofx.Battleships.Domain.DomainServices.GameService;

public interface IGameDomainService
{
    public Task<Game> CreateSinglePlayerGame(Guid playerId);
    public Task<Ship> AddBattleshipAsync(Guid gameId, Guid playerId, Point start, int length, LayoutDirection layoutDirection);
    public Task<Game> StartGame(Guid gameId);
    public Task<(bool isShipHit, bool isShipSunk)> AttackAndVerify(Guid gameId, Guid playerId, Point point);
}