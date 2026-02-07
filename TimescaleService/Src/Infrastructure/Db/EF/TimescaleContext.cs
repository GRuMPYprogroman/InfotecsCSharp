using Microsoft.EntityFrameworkCore;
using TimescaleService.Core.Domain;

namespace TimescaleService.Infrastructure.Db.EF;

public class TimescaleContext : DbContext
{
    public TimescaleContext(DbContextOptions<TimescaleContext> options) : base(options)
    {
    }

    public DbSet<AggregatedResult> Results { get; set; }
    
    public DbSet<Timescale> Values { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AggregatedResult>()
            .HasKey(x => x.FileName);

        modelBuilder.Entity<AggregatedResult>()
            .Property(x => x.FileName)
            .ValueGeneratedNever();
    }
}