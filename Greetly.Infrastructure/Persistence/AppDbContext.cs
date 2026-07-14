using Greetly.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Greetly.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Occasion> Occasions => Set<Occasion>();
    public DbSet<Style> Styles => Set<Style>();
    public DbSet<Greeting> Greetings => Set<Greeting>();
    public DbSet<GreetingFavorite> GreetingFavorites => Set<GreetingFavorite>();
    public DbSet<GreetingDraft> GreetingDrafts => Set<GreetingDraft>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GreetingFavorite>()
            .HasIndex(f => new { f.UserId, f.GreetingId })
            .IsUnique();

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
    }
}
