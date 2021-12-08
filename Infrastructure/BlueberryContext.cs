using System.Runtime.Serialization;

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

     public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {   
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
        {
            throw new NoDBConnectionException(e);
        }
    }
}

[Serializable]
public class NoDBConnectionException : Exception
{
    private static string DEFAULT_MSG = "There is no connection to the database. Make sure that it is running, and that the connection string is valid";
    public NoDBConnectionException() : base(DEFAULT_MSG)
    {
            // Intentionally Empty
    }

    public NoDBConnectionException(Exception innerException) : base(DEFAULT_MSG, innerException)
    {
            // Intentionally Empty
    }
}