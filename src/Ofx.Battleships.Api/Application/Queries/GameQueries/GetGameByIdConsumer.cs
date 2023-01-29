using MassTransit;
using MassTransit.Mediator;
using Ofx.Battleships.Api.SeedWork;
using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Infrastructure;

namespace Ofx.Battleships.Api.Application.Queries.GameQueries;

public record GetGameByIdQuery : Request<GetGameQueryResponse>
{
    public Guid GameId { get; set; }
}

public record GetGameQueryResponse : CommandQueryResponse
{
    public Game Game { get; set; }
}

public class GetGameByIdConsumer : IConsumer<GetGameByIdQuery>
{
    private readonly IGameRepository _GameRepository;

    public GetGameByIdConsumer(IGameRepository GameRepository)
    {
        _GameRepository = GameRepository;
    }
    
    public async Task Consume(ConsumeContext<GetGameByIdQuery> context)
    {
        try
        {
            var game = await _GameRepository.GetGameAsync(context.Message.GameId);
            await context.RespondAsync(new GetGameQueryResponse
            {
                Game = game
            });
        }
        catch (Exception e)
        {
            await context.RespondAsync(new GetGameQueryResponse
            {
                Errors = new List<string> { e.Message }
            });
        }
    }
}