namespace Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate.Exceptions;

public class PlayerExistsException : Exception
{
    public PlayerExistsException(string message) : base(message)
    {
    }
}