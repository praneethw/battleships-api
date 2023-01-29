using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;

namespace Ofx.Battleships.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static void SeedDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetService<BattleshipDbContext>();
        dbContext.Players.Add(
            new Player
            {
                Id = Guid.NewGuid(),
                Username = "computer"
            }
        );
        dbContext.SaveChanges();
    }
}