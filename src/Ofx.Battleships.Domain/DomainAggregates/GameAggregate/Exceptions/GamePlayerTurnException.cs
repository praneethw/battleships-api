namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate.Exceptions;

public class GamePlayerTurnException : Exception
{
    public GamePlayerTurnException(string message) : base(message)
    {
    }
}