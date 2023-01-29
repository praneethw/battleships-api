using MassTransit;
using MassTransit.Futures.Contracts;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ofx.Battleships.Api.Application.Commands.GameCommands;
using Ofx.Battleships.Api.Application.Queries.GameQueries;
using Ofx.Battleships.Api.SeedWork;
using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Infrastructure;

namespace Ofx.Battleships.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly BattleshipDbContext _battleshipDbContext;

    public GameController(IMediator mediator, BattleshipDbContext battleshipDbContext)
    {
        _mediator = mediator;
        _battleshipDbContext = battleshipDbContext;
    }
    [HttpGet("{gameId}")]
    public async Task<IActionResult> Get([FromRoute]GetGameByIdQuery getGameByIdQuery)
    {
        var getGameQueryResponse = await _mediator.SendRequest(getGameByIdQuery);
        if (!getGameQueryResponse.IsValid)
        {
            return BadRequest(getGameQueryResponse.Errors);
        }

        return Ok(getGameQueryResponse.Game);
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody]CreateSinglePlayerGameCommand createSinglePlayerGameCommand)
    {
        var createGameCommandResponse = await _mediator.SendRequest(createSinglePlayerGameCommand);
        if (!createGameCommandResponse.IsValid)
        {
            return BadRequest(createGameCommandResponse.Errors);
        }

        return Created($"{createGameCommandResponse.GameId}/{Url.Action(nameof(Get))}", createGameCommandResponse.GameId);
    }

    [HttpPost("{gameId}/Ship")]
    public async Task<IActionResult> AddShip([FromRoute]Guid gameId, [FromBody]AddShipCommand shipCommand)
    {
        shipCommand.GameId = gameId;
        var shipCommandResponse = await _mediator.SendRequest(shipCommand);
        if (!shipCommandResponse.IsValid)
        {
            return BadRequest(shipCommandResponse.Errors);
        }

        return Ok();
    }

    [HttpPost("{gameId}/Start")]
    public async Task<IActionResult> AddShip([FromRoute]Guid gameId, [FromBody]StartGameCommand startGameCommand)
    {
        startGameCommand.GameId = gameId;
        var startGameCommandResponse = await _mediator.SendRequest(startGameCommand);
        if (!startGameCommandResponse.IsValid)
        {
            return BadRequest(startGameCommandResponse.Errors);
        }

        return Ok();
    }
    
    [HttpPost("{gameId}/Attack")]
    public async Task<IActionResult> AddShip([FromRoute]Guid gameId, [FromBody]AttackShipCommand attackShipCommand)
    {
        attackShipCommand.GameId = gameId;
        var attackShipCommandResponse = await _mediator.SendRequest(attackShipCommand);
        if (!attackShipCommandResponse.IsValid)
        {
            return BadRequest(attackShipCommandResponse.Errors);
        }

        return Ok(new
        {
            attackShipCommandResponse.IsShipHit,
            attackShipCommandResponse.IsShipSunk
        });
    }
}