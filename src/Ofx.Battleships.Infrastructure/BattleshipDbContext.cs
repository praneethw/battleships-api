using Microsoft.EntityFrameworkCore;
using Ofx.Battleships.Domain.DomainAggregates.GameAggregate;
using Ofx.Battleships.Domain.DomainAggregates.PlayerAggregate;
using Ofx.Battleships.Domain.SeedWork;

namespace Ofx.Battleships.Infrastructure;

public class BattleshipDbContext : DbContext, IUnitOfWork
{
    public BattleshipDbContext(DbContextOptions<BattleshipDbContext> options) : base(options)
    {
    }
    
    public DbSet<Player> Players { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<Board> Board { get; set; }
    public DbSet<Coordinate> Coordinates { get; set; }
    public DbSet<Ship> Ships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasOne(g => g.ParticipantA);
        modelBuilder.Entity<Game>()
            .HasOne(g => g.ParticipantB);

        modelBuilder.Entity<Participant>()
            .HasOne(p => p.Player);
        modelBuilder.Entity<Participant>()
            .HasOne(p => p.Board);
        
        modelBuilder.Entity<Board>()
            .HasMany(b => b.Coordinates);
        
        modelBuilder.Entity<Coordinate>()
            .HasOne(c => c.Ship);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}