namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate.Exceptions;

public class GameCompletedException : Exception
{
    public GameCompletedException(string message) : base(message)
    {
    }
}