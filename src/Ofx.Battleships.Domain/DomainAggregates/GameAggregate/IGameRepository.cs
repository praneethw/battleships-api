using Ofx.Battleships.Domain.SeedWork;

namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate;

public interface IGameRepository : IRepository<Game>
{
    public Task<Game> AddAsync(Game game);
    public Task<Game> UpdateAsync(Game player);
    public Task<Game> GetGameAsync(Guid id);
}