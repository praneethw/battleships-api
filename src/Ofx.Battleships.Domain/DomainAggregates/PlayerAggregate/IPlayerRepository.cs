using Ofx.Battleships.Domain.SeedWork;

namespace Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;

public interface IPlayerRepository : IRepository<Player>
{
    public Task<Player> AddAsync(Player player);
    public Task<Player> UpdateAsync(Player player);
    public Task<Player> GetPlayerAsync(Guid id);
    public Task<Player> GetPlayerAsync(string username);
}