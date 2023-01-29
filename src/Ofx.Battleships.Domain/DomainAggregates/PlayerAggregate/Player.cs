using Ofx.Battleships.Domain.SeedWork;

namespace Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;

public class Player : Entity, IAggregateRoot
{
    public string Username { get; set; }
}