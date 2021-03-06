namespace Infrastructure.Tests;

public class TagRepositoryTests : IDisposable
{

    private readonly IBlueberryContext _context;
    private readonly TagRepository _repository;

    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<BlueberryContext>();
        builder.UseSqlite(connection);
        var context = new BlueberryContext(builder.Options);

        context.Database.EnsureCreated();

        var user = new User(name: "Jakob") { Id = 1 };
        context.Users.Add(user);
        context.Tags.AddRange(
            new Tag (name: "Docker") { Id = 1, Users = new HashSet<User>() { user } },
            new Tag (name: "Framework") { Id = 2 }
        );

        context.SaveChanges();

        _context = context;
        _repository = new TagRepository(_context);
    }

    [Fact]
    public async Task CreateGivenNotExistingTagReturnCreatedWithTag()
    {
        var tag = new TagCreateDto("Requirements Analysis Document");

        var created = await _repository.Create(tag);

        (Status, TagDto) expected = (Created, new TagDto(3, "Requirements Analysis Document"));

        Assert.Equal(expected, created);
    }

    [Fact]
    public async Task CreateGivenExistingTagReturnConflict()
    {
        var tag = new TagCreateDto("Framework");

        var created = await _repository.Create(tag);

        (Status, TagDto) expected = (Conflict, new TagDto(2, "Framework"));

        Assert.Equal(expected, created);
    }

    [Fact]
    public async Task ReadGivenExistingIdReturnTag()
    {
        var option = await _repository.Read(1);

        var expected = new TagDto(1, "Docker");

        Assert.Equal(expected, option.Value);
    }

    [Fact]
    public async Task ReadGivenNonExistingIdReturnsOptionNone()
    {
        var option = await _repository.Read(3);

        Assert.True(option.IsNone);
    }

    [Fact]
    public async Task ReadGivenNoIdReturnAllTags()
    {
        var tags = await _repository.Read();

        Assert.Collection(tags,
                        tag => Assert.Equal(new TagDto(1, "Docker"), tag),
                        tag => Assert.Equal(new TagDto(2, "Framework"), tag)
                        );
    }

    [Fact]
    public async Task DeleteGivenExistingIdReturnDeleted()
    {
        var response = await _repository.Delete(2);

        var entity = await _context.Tags.FindAsync(2);

        Assert.Equal(Deleted, response);
        Assert.Null(entity);
    }

    [Fact]
    public async Task DeleteGivenNonExistingIdReturnNotFound()
    {
        var response = await _repository.Delete(3);

        Assert.Equal(NotFound, response);
    }

    [Fact]
    public async Task DeleteGivenExistingTagWithUsersDoesNotDeleteAndReturnConflict()
    {
        var response = await _repository.Delete(1);

        var entity = await _context.Tags.FindAsync(1);

        Assert.Equal(Conflict, response);
        Assert.NotNull(entity);
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