using MassTransit;
using MassTransit.Mediator;
using Ofx.Battleships.Api.SeedWork;
using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;
using Ofx.Battleships.Domain.DomainServices.GameService;
using Ofx.Battleships.Domain.Exceptions;

namespace Ofx.Battleships.Api.Application.Commands.GameCommands;

public record AddShipCommand : Request<AddShipCommandResponse>
{
    public Guid GameId { get; set; }
    public Guid PlayerId { get; set; }
    public Point Point { get; set; }
    public int Length { get; set; }
    public LayoutDirection LayoutDirection { get; set; }
}

public record AddShipCommandResponse : CommandQueryResponse
{
    public Guid ShipId { get; set; }
}

public class AddShipConsumer : IConsumer<AddShipCommand>
{
    private readonly IGameDomainService _gameDomainService;
    private readonly IGameRepository _gameRepository;

    public AddShipConsumer(IGameDomainService gameDomainService, IGameRepository gameRepository)
    {
        _gameDomainService = gameDomainService;
        _gameRepository = gameRepository;
    }
    
    public async Task Consume(ConsumeContext<AddShipCommand> context)
    {
        try
        {
            var ship = await _gameDomainService.AddBattleshipAsync(context.Message.GameId, context.Message.PlayerId, context.Message.Point, context.Message.Length, context.Message.LayoutDirection);

            await context.RespondAsync(new AddShipCommandResponse
            {
                ShipId = ship.Id
            });
        
            await _gameRepository.UnitOfWork.SaveEntitiesAsync();
        }
        catch (Exception e)
        {
            await context.RespondAsync(new AddShipCommandResponse
            {
                Errors = new List<string> { e.Message }
            });
        }
    }
}