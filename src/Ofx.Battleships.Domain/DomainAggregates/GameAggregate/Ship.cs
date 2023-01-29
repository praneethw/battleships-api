using Ofx.Battleships.Domain.SeedWork;

namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate;

public class Ship : Entity
{
    public bool IsSunk { get; set; }
}