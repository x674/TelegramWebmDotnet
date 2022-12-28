using Microsoft.EntityFrameworkCore;

namespace telegramWebm.Database;

public class MediaRepository : DbContext
{
    public DbSet<Files> Files { get; set; }
    
    //public DbSet<Threads> Threads { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql($"Host={Environment.GetEnvironmentVariable("dbHost")};Username=postgres;Password=admin;Database=mydatabase");
}
