using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;

namespace Ofx.Battleships.Domain.DomainServices.PlayerService;

public interface IPlayerDomainService
{
    public Task<Player> CreatePlayerAsync(string username);
}