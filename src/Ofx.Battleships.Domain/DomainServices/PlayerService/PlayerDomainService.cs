using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate.Exceptions;

namespace Ofx.Battleships.Domain.DomainServices.PlayerService;

public class PlayerDomainService : IPlayerDomainService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerDomainService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }
    
    public async Task<Player> CreatePlayerAsync(string username)
    {
        var existingPlayer = await _playerRepository.GetPlayerAsync(username);
        if (existingPlayer != null)
        {
            throw new PlayerExistsException($"Player with {username} already exists.");
        }
        
        return await _playerRepository.AddAsync(new Player
        {
            Username = username
        });
    }
}