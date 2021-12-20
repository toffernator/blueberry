namespace blueberry.Infrastructure;

/// <summary>Defines resources available in the database</summary>
public interface IBlueberryContext : IDisposable
{
    DbSet<User> Users { get; }
    DbSet<Material> Materials { get; }
    DbSet<Tag> Tags { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}