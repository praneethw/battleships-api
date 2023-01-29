using Microsoft.EntityFrameworkCore;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Domain.SeedWork;

namespace Ofx.Battleships.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly BattleshipDbContext _battleshipDbContext;
    public IUnitOfWork UnitOfWork => _battleshipDbContext;

    public PlayerRepository(BattleshipDbContext battleshipDbContext)
    {
        _battleshipDbContext = battleshipDbContext ?? throw new ArgumentNullException(nameof(battleshipDbContext));
    }
    
    public Task<Player> AddAsync(Player player)
    {
        return Task.FromResult(_battleshipDbContext
            .Players
            .Add(player)
            .Entity);
    }

    public Task<Player> UpdateAsync(Player player)
    {
        return Task.FromResult(_battleshipDbContext
            .Players
            .Update(player)
            .Entity);
    }

    public async Task<Player?> GetPlayerAsync(Guid id)
    {
        return await _battleshipDbContext
            .Players
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Player?> GetPlayerAsync(string username)
    {
        return await _battleshipDbContext
            .Players
            .Where(p => p.Username == username)
            .FirstOrDefaultAsync();
    }
}