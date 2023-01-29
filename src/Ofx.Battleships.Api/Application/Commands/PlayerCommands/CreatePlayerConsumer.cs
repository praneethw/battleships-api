using System.ComponentModel.DataAnnotations;
using MassTransit;
using MassTransit.Mediator;
using Ofx.Battleships.Api.SeedWork;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate.Exceptions;
using Ofx.Battleships.Domain.DomainServices.PlayerService;

namespace Ofx.Battleships.Api.Application.Commands.PlayerCommands;
public record CreatePlayerCommand : Request<CreatePlayerCommandResponse>
{
    [Required]
    public string Username { get; init; }
}

public record CreatePlayerCommandResponse : CommandQueryResponse
{
    public Guid PlayId { get; init; }
}

public class CreatePlayerConsumer : IConsumer<CreatePlayerCommand>
{
    private readonly IPlayerDomainService _playerDomainService;
    private readonly IPlayerRepository _playerRepository;

    public CreatePlayerConsumer(
        IPlayerDomainService playerDomainService,
        IPlayerRepository playerRepository)
    {
        _playerDomainService = playerDomainService;
        _playerRepository = playerRepository;
    }
    
    public async Task Consume(ConsumeContext<CreatePlayerCommand> context)
    {
        try
        {
            var player = await _playerDomainService.CreatePlayerAsync(context.Message.Username);
            
            await context.RespondAsync(new CreatePlayerCommandResponse
            {
                PlayId = player.Id
            });
        
            await _playerRepository.UnitOfWork.SaveEntitiesAsync();
        }
        catch (Exception e)
        {
            await context.RespondAsync(new CreatePlayerCommandResponse
            {
                Errors = new List<string> { e.Message }
            });
        }
    }
}