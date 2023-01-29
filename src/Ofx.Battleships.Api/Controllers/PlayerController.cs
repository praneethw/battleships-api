using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;
using Ofx.Battleships.Api.Application.Commands.PlayerCommands;
using Ofx.Battleships.Api.Application.Queries.PlayerQueries;

namespace Ofx.Battleships.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlayerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{PlayerId}")]
    public async Task<IActionResult> Get([FromRoute]GetPlayerByIdQuery getPlayerByIdQuery)
    {
        var getPlayerByIdQueryResponse = await _mediator.SendRequest(getPlayerByIdQuery);
        if (getPlayerByIdQueryResponse?.Player == null)
        {
            return BadRequest();
        }

        return Ok(getPlayerByIdQueryResponse.Player);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreatePlayerCommand createPlayerCommand)
    {
        var createPlayerCommandResponse = await _mediator.SendRequest(createPlayerCommand);
        if (!createPlayerCommandResponse.IsValid)
        {
            return BadRequest(createPlayerCommandResponse.Errors);
        }

        return Created($"{createPlayerCommandResponse.PlayId}/{Url.Action(nameof(Get))}", createPlayerCommandResponse.PlayId);
    }
}