using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ofx.Battleships.Api.Application.Commands.GameCommands;
using Ofx.Battleships.Api.Application.Commands.PlayerCommands;
using Ofx.Battleships.Api.Application.Queries.GameQueries;
using Ofx.Battleships.Api.Application.Queries.PlayerQueries;
using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Domain.DomainServices.GameService;
using Ofx.Battleships.Domain.DomainServices.PlayerService;
using Ofx.Battleships.Infrastructure;
using Ofx.Battleships.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BattleshipDbContext>(config => config.UseInMemoryDatabase("battleshipdb"));
builder.Services.AddMediator(config =>
{
    config.AddConsumer<CreatePlayerConsumer>();
    config.AddConsumer<GetPlayerByIdConsumer>();

    config.AddConsumer<CreateSinglePlayerGameConsumer>();
    config.AddConsumer<AddShipConsumer>();
    config.AddConsumer<StartGameConsumer>();
    config.AddConsumer<AttackShipConsumer>();
    config.AddConsumer<GetGameByIdConsumer>();
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IPlayerDomainService, PlayerDomainService>();
builder.Services.AddTransient<IGameDomainService, GameDomainService>();

builder.Services.AddTransient<IPlayerRepository, PlayerRepository>();
builder.Services.AddTransient<IGameRepository, GameRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase("/api/v1/");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

BattleshipDbContextSeeder.SeedDatabase(app.Services.CreateScope().ServiceProvider.GetService<BattleshipDbContext>());

app.Run();