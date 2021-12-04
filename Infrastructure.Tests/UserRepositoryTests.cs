namespace Infrastructure.Tests;

public class UserRepositoryTests : IDisposable
{

    private readonly IBlueberryContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<BlueberryContext>();
        builder.UseSqlite(connection);
        var context = new BlueberryContext(builder.Options);

        context.Database.EnsureCreated();

        context.Tags.AddRange(new Tag{ Id = 1, Name = "Docker"}, new Tag{ Id = 2 , Name = "Framework"});
        context.Users.AddRange(
                            new User{ Id = 1, Name = "Jalle" },
                            new User{ Id = 2, Name = "Kobo" , Tags = new HashSet<Tag>(){context.Tags.Find(1), context.Tags.Find(2)}}
                        );

        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task CreateCreatesNewUserWithGeneratedId ()
    {
        var user = new UserCreateDto("Boerge", new PrimitiveCollection<string>() {"vscode", "Microsoft"});

        var created = await _repository.Create(user);

        UserDto expected = new UserDto(3, "Boerge", new PrimitiveCollection<string>() {"vscode", "Microsoft"});

        Assert.Equal(expected, created);
    }

    [Fact]
    public async Task ReadReturnAllUsers()
    {
        var users = await _repository.Read();
        
        var expected = new PrimitiveCollection<UserDto>()
        { 
            new UserDto(1, "Jalle", new PrimitiveCollection<string>()),
            new UserDto(2, "Kobo", new PrimitiveCollection<string>() { "Docker", "Framework" })
        };

        Assert.Equal(expected, users);
    }

    [Fact]
    public async Task ReadGivenExistingIdReturnOptionAndUserDto()
    {
        var option = await _repository.Read(2);
        
        var expected = new UserDto(2, "Kobo", new PrimitiveCollection<string>() {"Docker", "Framework"});

        Assert.Equal(expected, option.Value);
    }

    [Fact]
    public async Task ReadGivenNonExistingIdReturnOptionIsNone()
    {
        var option = await _repository.Read(3);

        Assert.True(option.IsNone);
    }

    [Fact]
    public async Task UpdateGivenExistingIdUpdateUserAndReturnUpdated()
    {
        var userUpdateDto = new UserUpdateDto(1, new PrimitiveCollection<string>() {"Docker", "Framework", "dotnet"});

        var update = await _repository.Update(1, userUpdateDto);

        var option = await _repository.Read(1);

        var expected = new UserDto(1, "Jalle", new PrimitiveCollection<string>() { "Docker", "Framework", "dotnet" });
        
        Assert.Equal(Updated, update);
        Assert.Equal(expected, option.Value);
    }

    [Fact]
    public async Task UpdateGivenNonExistingIdReturnNotFound()
    {

        var userUpdateDto = new UserUpdateDto(3, new PrimitiveCollection<string>() );

        var status = await _repository.Update(3, userUpdateDto);

        Assert.Equal(NotFound, status);
    }


    [Fact]
    public async Task DeleteGivenExistingIdReturnDeleted()
    {
        var status = await _repository.Delete(2);

        var entity = await _context.Users.FindAsync(2);

        Assert.Null(entity);
        Assert.Equal(Deleted, status);
    }

    [Fact]
    public async Task DeleteGivenExistingIdReturnNotFound()
    {
        var status = await _repository.Delete(3);

        Assert.Equal(NotFound, status);
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