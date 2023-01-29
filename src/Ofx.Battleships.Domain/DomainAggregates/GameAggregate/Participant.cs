using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Domain.SeedWork;

namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate;

public class Participant : Entity
{
    public Player Player { get; set; }
    public Board Board { get; set; }
}