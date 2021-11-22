namespace Infrastructure.Tests;

public class TagRepositoryTests : IDisposable
{

    private readonly IBlueberryContext _context;
    private readonly TagRepository _repository;

    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory");
        connection.Open();
        var builder = new DbContextOptionsBuilder<BlueberryContext>();
        builder.UseSqlite(connection);
        var context = new BlueberryContext(builder.Options);

        context.Database.EnsureCreated();

        //Will be necessary later (i suppose)
        context.Tags.AddRange(new Tag("Docker") { Id = 1 }, new Tag("Framework") { Id = 2 });
        context.Users.Add(new User { Id = 1, Name = "Teapot" , Interests = new HashSet<Tag>() });
        context.SaveChanges();

        _context = context;
        _repository = new TagRepository(_context);
    }

    [Fact]
    public async Task Create_given_Tag_return_Created_with_Tag()
    {
        var tag = new TagCreateDto("Requirements Analysis Document");

        var created = await _repository.Create(tag);

        Assert.Equal(new TagDto(3, "Requirements Analysis Document"), created);
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

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}