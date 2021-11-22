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
    public async Task Create_given_not_existing_Tag_return_Created_with_Tag()
    {
        var tag = new TagCreateDto("Requirements Analysis Document");

        var created = await _repository.Create(tag);
        (Status, TagDto) expected = (Created, new TagDto(3, "Requirements Analysis Document"));

        Assert.Equal(expected, created);
    }

    [Fact]
    public async Task Create_given_existing_Tag_return_return_Conflict()
    {
        var tag = new TagCreateDto("Framework");

        var created = await _repository.Create(tag);

        (Status, TagDto) expected = (Conflict, new TagDto(2, "Framework"));

        Assert.Equal(expected, created);
    }

    [Fact]
    public async Task Read_given_existing_id_return_Tag()
    {
        var option = await _repository.Read(1);

        var expected = new TagDto(1, "Docker");

        Assert.Equal(expected, option.Value);
    }

    [Fact]
    public async Task Read_given_non_existing_id_returns_option_None()
    {
        var option = await _repository.Read(3);

        Assert.True(option.IsNone);
    }

    [Fact]
    public async Task Read_given_no_id_return_all_Tags()
    {
        var tags = await _repository.Read();

        Assert.Collection(tags,
                        tag => Assert.Equal(new TagDto(1,"Docker"), tag),
                        tag => Assert.Equal(new TagDto(2,"Framework"), tag)
                        );
    }

    [Fact]
    public async Task Delete_given_existing_id_return_Deleted()
    {
        var response = await _repository.Delete(2);

        var entity = await _context.Tags.FindAsync(2);

        Assert.Equal(Deleted, response);
        Assert.NotNull(entity);
    }

    [Fact]
    public async Task Delete_given_non_existing_id_return_Conflict_and_entity()
    {
        var response = await _repository.Delete(2);

        var entity = await _context.Tags.FindAsync(2);

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

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}