namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate.Exceptions;

public class PlayerNotSetException : Exception
{
    public PlayerNotSetException(string message) : base(message)
    {
    }
}