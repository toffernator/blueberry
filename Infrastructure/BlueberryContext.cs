namespace blueberry.Infrastructure;

public class BlueberryContext : DbContext, IBlueberryContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Material> Materials => Set<Material>();

    public DbSet<Tag> Interests => Set<Tag>();

    public BlueberryContext(DbContextOptions<BlueberryContext> options) : base(options) 
    {
        // Intentionally empty
    }

    public Task<int> SaveChanges(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}