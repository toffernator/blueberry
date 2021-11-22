namespace blueberry.Infrastructure;

public interface IBlueberryContext : IDisposable
{
    DbSet<User> Users { get; }
    DbSet<Material> Materials { get; }

    DbSet<Tag> Interests { get; }
    Task<int> SaveChanges(CancellationToken cancellationToken = default);
}