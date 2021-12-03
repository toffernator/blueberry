using Microsoft.Extensions.DependencyInjection;

namespace blueberry.Infrastructure;

public class BlueberryContext : DbContext, IBlueberryContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<Tag> Tags => Set<Tag>();
    public BlueberryContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>();

        modelBuilder.Entity<Material>();

        modelBuilder.Entity<Tag>()
                    .HasIndex(t => t.Name)
                    .IsUnique();
    }

}