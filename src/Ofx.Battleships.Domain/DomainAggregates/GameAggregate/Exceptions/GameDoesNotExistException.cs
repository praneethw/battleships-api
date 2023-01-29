namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate.Exceptions;

public class GameDoesNotExistException : Exception
{
    public GameDoesNotExistException(string message) : base(message)
    {
    }
}