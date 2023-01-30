using FluentAssertions;
using Moq;
using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Domain.DomainServices.GameService;

namespace Ofx.Battleships.Domain.Tests;

public class GameDomainServiceTests
{
    [Fact]
    public async void When_Create_Single_Player_Game_Is_Created_Then_Game_With_Participants_Is_Generated()
    {
        // Arrange
        var human_player = new Player { Id = Guid.NewGuid(), Username = "Player 1" };
        var computer_player = new Player { Id = Guid.NewGuid(), Username = "computer" };
        
        var mockPlayerRepository = new Mock<IPlayerRepository>();
        mockPlayerRepository
            .Setup(x => x.GetPlayerAsync(It.IsAny<Guid>()))
            .ReturnsAsync(human_player);
        mockPlayerRepository
            .Setup(x => x.GetPlayerAsync(It.Is<string>(s => s == "computer")))
            .ReturnsAsync(computer_player);

        var mockGameRepository = new Mock<IGameRepository>();
        mockGameRepository
            .Setup(s => s.AddAsync(It.IsAny<Game>()))
            .ReturnsAsync((Game game) => game);

        // Act
        var gameDomainService = new GameDomainService(mockPlayerRepository.Object, mockGameRepository.Object);
        var game = await gameDomainService.CreateSinglePlayerGame(human_player.Id);

        // Assert
        game.ParticipantA.Should().NotBeNull();
        game.ParticipantA.Player.Should().NotBeNull();
        game.ParticipantA.Board.Should().NotBeNull();
        game.ParticipantA.Player.Should().Be(human_player);
        game.ParticipantA.Board.Coordinates.Should().HaveCount(100);

        game.ParticipantB.Should().NotBeNull();
        game.ParticipantB.Player.Should().NotBeNull();
        game.ParticipantB.Board.Should().NotBeNull();
        game.ParticipantB.Player.Should().Be(computer_player);
        game.ParticipantB.Board.Coordinates.Should().HaveCount(100);

        game.IsSinglePlayer.Should().BeTrue();
        game.Status.Should().Be(Status.NotStarted);
    }

    [Fact]
    public async void When_Battleship_Is_Added_Then_Game_Board_Is_Updated()
    {
        // Arrange
        var human_player = new Player { Id = Guid.NewGuid(), Username = "Player 1" };
        var computer_player = new Player { Id = Guid.NewGuid(), Username = "computer" };
        var game = new Game { Id = Guid.NewGuid() };
        game.AddParticipantA(human_player);
        game.AddParticipantB(computer_player);

        var mockPlayerRepository = new Mock<IPlayerRepository>();
        var mockGameRepository = new Mock<IGameRepository>();
        mockGameRepository
            .Setup(s => s.GetGameAsync(It.IsAny<Guid>()))
            .ReturnsAsync(game);

        // Act
        var gameDomainService = new GameDomainService(mockPlayerRepository.Object, mockGameRepository.Object);
        var ship = await gameDomainService.AddBattleshipAsync(game.Id, human_player.Id, new Point() { X = 1, Y = 1 }, 5,
            LayoutDirection.Horizontal);

        // Assert
        game.ParticipantA.Should().NotBeNull();
        game.ParticipantA.Player.Should().NotBeNull();
        game.ParticipantA.Board.Should().NotBeNull();
        game.ParticipantA.Player.Should().Be(human_player);

        var coordinates = game.ParticipantA.Board.Coordinates.Where(c => c.Ship != null).ToList();
        coordinates.Should().HaveCount(5);
        coordinates.Should().Contain(x => x.X >= 1 && x.X <= 5 && x.Y == 1);
    }
}