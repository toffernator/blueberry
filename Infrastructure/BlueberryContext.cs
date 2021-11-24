namespace blueberry.Infrastructure;

public class BlueberryContext : DbContext, IBlueberryContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Material> Materials => Set<Material>();

    public DbSet<Tag> Tags => Set<Tag>();

    public BlueberryContext(DbContextOptions<BlueberryContext> options) : base(options) 
    {
        // Intentionally empty
    }
}