using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Domain.SeedWork;

namespace Ofx.Battleships.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly BattleshipDbContext _battleshipDbContext;
    public IUnitOfWork UnitOfWork => _battleshipDbContext;

    public GameRepository(BattleshipDbContext battleshipDbContext)
    {
        _battleshipDbContext = battleshipDbContext ?? throw new ArgumentNullException(nameof(battleshipDbContext));
    }
    
    public Task<Game> AddAsync(Game game)
    {
        return Task.FromResult(_battleshipDbContext
            .Games
            .Add(game)
            .Entity);
    }

    public Task<Game> UpdateAsync(Game game)
    {
        return Task.FromResult(_battleshipDbContext
            .Games
            .Update(game)
            .Entity);
    }
    
    public async Task<Game?> GetGameAsync(Guid id)
    {
        var game = await _battleshipDbContext
            .Games
            .Include(g => g.ParticipantA)
            .ThenInclude(p => p.Player)
            .Include(g => g.ParticipantA.Board)
            .ThenInclude(b => b.Coordinates)
            .ThenInclude(c => c.Ship)
            .Include(g => g.ParticipantB)
            .ThenInclude(p => p.Player)
            .Include(g => g.ParticipantB.Board)
            .ThenInclude(b => b.Coordinates)
            .ThenInclude(c => c.Ship)
            .Where(g => g.Id == id)
            .FirstOrDefaultAsync();
        
        return game;
    }
}