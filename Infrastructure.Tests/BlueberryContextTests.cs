namespace Infrastructure.Tests;

public class BlueberryContextTests : IDisposable
{

    private readonly IBlueberryContext _context;

    public BlueberryContextTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<BlueberryContext>();
        builder.UseSqlite(connection);
        var context = new BlueberryContext(builder.Options);

        _context = context;
    }

    [Fact]
    public async Task RepoGivenNoDatabaseThrowsNoDbConnectionException()
    {
        _context.Tags.Add(new Tag{Name = "Test"});
        await Assert.ThrowsAsync<NoDBConnectionException>(async () => await _context.SaveChangesAsync());
    }

    private bool disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}