using MassTransit;
using MassTransit.Mediator;
using Ofx.Battleships.Api.SeedWork;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;

namespace Ofx.Battleships.Api.Application.Queries.PlayerQueries;

public record GetPlayerByIdQuery : Request<GetPlayerQueryResponse>
{
    public Guid Id { get; set; }
}

public record GetPlayerQueryResponse : CommandQueryResponse
{
    public Player Player { get; set; }
}

public class GetPlayerByIdConsumer : IConsumer<GetPlayerByIdQuery>
{
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerByIdConsumer(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task Consume(ConsumeContext<GetPlayerByIdQuery> context)
    {
        try
        {
            var player = await _playerRepository.GetPlayerAsync(context.Message.Id);
            await context.RespondAsync(new GetPlayerQueryResponse
            {
                Player = player
            });
        }
        catch (Exception e)
        {
            await context.RespondAsync(new GetPlayerQueryResponse
            {
                Errors = new List<string> { e.Message }
            });
        }
    }
}