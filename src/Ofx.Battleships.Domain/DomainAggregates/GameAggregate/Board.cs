using Ofx.Battleships.Domain.Exceptions;
using Ofx.Battleships.Domain.SeedWork;

namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate;

public class Board : Entity
{
    public List<Coordinate> Coordinates { get; set; } = new();
    public LayoutDirection? LayoutDirection { get; set; }

    public Ship AddShip(Point start, int length, LayoutDirection layoutDirection)
    {
        var ship = new Ship();
        if (--length < 0) throw new DomainException("Length cannot be less than 1");
        if (LayoutDirection != null && LayoutDirection != layoutDirection) throw new DomainException("Invalid layout. Current board is configured to {LayoutDirection.Value} layout.");
        
        switch (layoutDirection)
        {
            case GameAggregate.LayoutDirection.Horizontal when start.Y + length > Coordinates.Max(c => c.Y):
                throw new DomainException("Ship length exceeds board horizontal boundary.");
            case GameAggregate.LayoutDirection.Horizontal when Coordinates.Any(c => c.X == start.X && c.Y >= start.Y && c.Y <= start.Y + length && c.Ship != null):
                throw new DomainException("A ship has already been placed within the specified coordinates");
            case GameAggregate.LayoutDirection.Horizontal:
            {
                for (var y = start.Y; y <= start.Y + length; y++)
                {
                    var coordinate = Coordinates.First(c => c.X == start.X && c.Y == y);
                    coordinate.Ship = ship;
                }

                break;
            }
            case GameAggregate.LayoutDirection.Vertical when start.X + length > Coordinates.Max(c => c.X):
                throw new DomainException("Ship length exceeds board vertical boundary.");
            case GameAggregate.LayoutDirection.Vertical when Coordinates.Any(c => c.Y == start.Y && c.X >= start.X && c.X <= start.X + length && c.Ship != null):
                throw new DomainException("A ship has already been placed within the specified coordinates");
            case GameAggregate.LayoutDirection.Vertical:
            {
                for (var x = start.X; x <= start.X + length; x++)
                {
                    var coordinate = Coordinates.First(c => c.X == x && c.Y == start.Y);
                    coordinate.Ship = ship;
                }

                break;
            }
            default:
                throw new DomainException("Failed to place ship/");
        }
        
        LayoutDirection = layoutDirection;
        return ship;
    }

    public void Attack(Point attackPoint)
    {
        // Mark coordinate as hit.
        var coordinate = Coordinates.First(c => c.X == attackPoint.X && c.Y == attackPoint.Y);
        coordinate.Hit = true;

        if (coordinate.Ship == null) return;
        
        // If a coordinate has a ship, mark ship as sunk when all ship coordinates are hit.
        var shipCoordinates = Coordinates.Where(c => c.Ship?.Id == coordinate.Ship.Id);
        if (shipCoordinates.All(c => c.Hit))
        {
            coordinate.Ship.IsSunk = true;
        }
    }
}