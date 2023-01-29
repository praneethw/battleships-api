using Ofx.Battleships.Domain.SeedWork;

namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate;

public class Coordinate : Entity
{
    public int X { get; set; }
    public int Y { get; set; }
    public Ship? Ship { get; set; }
    public bool Hit { get; set; }
}