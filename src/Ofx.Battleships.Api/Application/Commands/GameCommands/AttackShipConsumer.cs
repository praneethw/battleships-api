using MassTransit;
using MassTransit.Mediator;
using Ofx.Battleships.Api.SeedWork;
using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;
using Ofx.Battleships.Domain.DomainServices.GameService;
using Ofx.Battleships.Domain.Exceptions;

namespace Ofx.Battleships.Api.Application.Commands.GameCommands;

public record AttackShipCommand : Request<AttackShipCommandResponse>
{
    public Guid GameId { get; set; }
    public Guid PlayerId { get; set; }
    public Point Point { get; set; }
}

public record AttackShipCommandResponse : CommandQueryResponse
{
    public bool IsShipHit { get; set; }
    public bool IsShipSunk { get; set; }
}

public class AttackShipConsumer : IConsumer<AttackShipCommand>
{
    private readonly IGameDomainService _gameDomainService;
    private readonly IGameRepository _gameRepository;

    public AttackShipConsumer(IGameDomainService gameDomainService, IGameRepository gameRepository)
    {
        _gameDomainService = gameDomainService;
        _gameRepository = gameRepository;
    }
    
    public async Task Consume(ConsumeContext<AttackShipCommand> context)
    {
        try
        {
            var attackStatus = await _gameDomainService.AttackAndVerify(context.Message.GameId, context.Message.PlayerId, context.Message.Point);

            await context.RespondAsync(new AttackShipCommandResponse
            {
                IsShipHit = attackStatus.isShipHit,
                IsShipSunk = attackStatus.isShipSunk
            });
        
            await _gameRepository.UnitOfWork.SaveEntitiesAsync();
        }
        catch (Exception e)
        {
            await context.RespondAsync(new AttackShipCommandResponse
            {
                Errors = new List<string> { e.Message }
            });
        }
    }
}