using Ofx.Battleships.Domain.DomainAggregates.GameAggregate.Exceptions;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Domain.Exceptions;
using Ofx.Battleships.Domain.SeedWork;

namespace Ofx.Battleships.Domain.DomainAggregates.GameAggregate;

public class Game : Entity, IAggregateRoot
{
    public Participant ParticipantA { get; set; }
    public Participant ParticipantB { get; set; }
    public Participant PlayingParticipant { get; set; }
    public bool IsSinglePlayer { get; set; }
    public Status Status { get; set; }

    public void AddParticipantA(Player player)
    {
        ParticipantA = Status switch
        {
            Status.Started => throw new GameStartedException(
                "Game is already started. Cannot add or change Participant A."),
            Status.Completed => throw new GameCompletedException(
                "Game is already completed. Cannot add or change Participant A."),
            _ => new Participant { Board = InitializeNewBoard(), Player = player }
        };
    }
    
    public void AddParticipantB(Player player)
    {
        ParticipantB = Status switch
        {
            Status.Started => throw new GameStartedException(
                "Game is already started. Cannot add or change Participant B."),
            Status.Completed => throw new GameCompletedException(
                "Game is already completed. Cannot add or change Participant B."),
            _ => new Participant { Board = InitializeNewBoard(), Player = player }
        };
    }

    public void StartGame()
    {
        if (Status == Status.Started) throw new GameStartedException("Game is already started.");
        if (Status == Status.Completed) throw new GameCompletedException("Game is already completed.");

        PlayingParticipant = ParticipantA;
        Status = Status.Started;
    }

    public Ship AddShip(Guid playerId, Point start, int length, LayoutDirection layoutDirection)
    {
        var participant = ParticipantA.Player.Id == playerId ? ParticipantA :
            ParticipantB.Player.Id == playerId ? ParticipantB : null;
        if (participant == null) throw new DomainException($"Invalid player ID {playerId}.");

        return participant.Board.AddShip(start, length, layoutDirection);
    }

    private Board InitializeNewBoard()
    {
        var coordinates = new List<Coordinate>();
        for (var x = 0; x < 10; x++)
        {
            for (var y = 0; y < 10; y++)
            {
                coordinates.Add(new Coordinate
                {
                    X = x,
                    Y = y
                });
            }
        }

        return new Board
        {
            Coordinates = coordinates
        };
    }
}