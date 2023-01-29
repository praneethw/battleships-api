using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;

namespace Ofx.Battleships.Infrastructure;

public static class BattleshipDbContextSeeder
{
    public static void SeedDatabase(BattleshipDbContext battleshipDbContext)
    {
        battleshipDbContext.Players.Add(
            new Player
            {
                Id = Guid.NewGuid(),
                Username = "computer"
            }
        );
        battleshipDbContext.SaveChanges();
    }
}