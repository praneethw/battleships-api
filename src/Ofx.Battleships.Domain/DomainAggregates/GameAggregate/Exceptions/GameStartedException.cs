namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate.Exceptions;

public class GameStartedException : Exception
{
    public GameStartedException(string message) : base(message)
    {
    }
}