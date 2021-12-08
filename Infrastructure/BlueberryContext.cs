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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {   
        Task<int> token;
        try
        {
            token = base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException db)
        {
            throw new NoDBConnectionException();
        }
        return token;
    }
}

[Serializable]
public class NoDBConnectionException : Exception
{
    public NoDBConnectionException()
    {
        Console.WriteLine("YOOOOOO!");
    }

    public NoDBConnectionException(string? message) : base(message)
    {
    }

    public NoDBConnectionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NoDBConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}