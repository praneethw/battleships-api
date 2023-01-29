namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate.Exceptions;

public class LayoutException : Exception
{
    public LayoutException(string message) : base(message)
    {
    }
}