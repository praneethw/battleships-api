namespace Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate.Exceptions;

public class PlayerDoesNotExistException : Exception
{
    public PlayerDoesNotExistException(string message) : base(message)
    {
    }
}