using MassTransit;
using MassTransit.Mediator;
using Ofx.Battleships.Api.SeedWork;
using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;
using Ofx.Battleships.Domain.DomainServices.GameService;
using Ofx.Battleships.Domain.Exceptions;

namespace Ofx.Battleships.Api.Application.Commands.GameCommands;

public record StartGameCommand : Request<StartGameCommandResponse>
{
    public Guid GameId { get; set; }
}

public record StartGameCommandResponse : CommandQueryResponse
{
    public Guid GameId { get; set; }
}

public class StartGameConsumer : IConsumer<StartGameCommand>
{
    private readonly IGameDomainService _gameDomainService;
    private readonly IGameRepository _gameRepository;

    public StartGameConsumer(IGameDomainService gameDomainService, IGameRepository gameRepository)
    {
        _gameDomainService = gameDomainService;
        _gameRepository = gameRepository;
    }
    
    public async Task Consume(ConsumeContext<StartGameCommand> context)
    {
        try
        {
            var game = await _gameDomainService.StartGame(context.Message.GameId);

            await context.RespondAsync(new StartGameCommandResponse
            {
                GameId = game.Id
            });
        
            await _gameRepository.UnitOfWork.SaveEntitiesAsync();
        }
        catch (Exception e)
        {
            await context.RespondAsync(new StartGameCommandResponse
            {
                Errors = new List<string> { e.Message }
            });
        }
    }
}