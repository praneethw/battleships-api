using MassTransit;
using MassTransit.Mediator;
using Ofx.Battleships.Api.SeedWork;
using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Domain.DomainServices.GameService;
using Ofx.Battleships.Domain.DomainServices.PlayerService;

namespace Ofx.Battleships.Api.Application.Commands.GameCommands;

public record CreateSinglePlayerGameCommand : Request<CreateSinglePlayerGameCommandResponse>
{
    public string Name { get; set; }
    public Guid PlayerId { get; set; }
}

public record CreateSinglePlayerGameCommandResponse : CommandQueryResponse
{
    public Guid GameId { get; set; }
}

public class CreateSinglePlayerGameConsumer : IConsumer<CreateSinglePlayerGameCommand>
{
    private readonly IGameDomainService _gameDomainService;
    private readonly IGameRepository _gameRepository;

    public CreateSinglePlayerGameConsumer(IGameDomainService gameDomainService, IGameRepository gameRepository)
    {
        _gameDomainService = gameDomainService;
        _gameRepository = gameRepository;
    }
    
    public async Task Consume(ConsumeContext<CreateSinglePlayerGameCommand> context)
    {
        try
        {
            var game = await _gameDomainService.CreateSinglePlayerGame(context.Message.PlayerId);
            
            await context.RespondAsync(new CreateSinglePlayerGameCommandResponse()
            {
                GameId = game.Id
            });
        
            await _gameRepository.UnitOfWork.SaveEntitiesAsync();
        }
        catch (Exception e)
        {
            await context.RespondAsync(new CreateSinglePlayerGameCommandResponse
            {
                Errors = new List<string> { e.Message }
            });
        }
    }
}