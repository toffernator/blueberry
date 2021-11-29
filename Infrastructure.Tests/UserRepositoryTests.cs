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
        context.Users.AddRange(new User{ Id = 1, Name = "Jalle" }, new User{ Id = 2, Name = "Kobo" , Tags = new HashSet<Tag>(){context.Tags.Find(1), context.Tags.Find(2)} });

        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task CreateCreatesNewUserWithGeneratedId ()
    {
        var user = new UserCreateDto("Boerge", new HashSet<string>(){"vscode", "Microsoft"});

        var created = await _repository.Create(user);

        UserDto expected = new UserDto(3, "Boerge", new HashSet<string>(){"vscode", "Microsoft"});

       Assert.Equal(expected.Id, created.Id);
       Assert.Equal(expected.Name, created.Name);
       Assert.True(created.Tags.SetEquals(expected.Tags));
    }

[Fact]
public async Task ReadReturnAllUsers()
{
    var users = await _repository.Read();
    
    var expected = new HashSet<UserDto>(){ new UserDto( 1, "Jalle", new HashSet<string>() ), new UserDto( 2, "Kobo", new HashSet<string>(){ "Docker", "Framework" })};

    var equals = UsersEquals(users, expected);
    Assert.True(equals);
}

[Fact]
public async Task ReadGivenExistingIdReturnOptionAndUserDto()
{
    var option = await _repository.Read(2);
    
    var expected = new UserDto(2, "Kobo", new HashSet<string>(){"Docker", "Framework"});

    Assert.Equal(expected.Id, option.Value.Id);
    Assert.Equal(expected.Name, option.Value.Name);
    Assert.True(option.Value.Tags.SetEquals(expected.Tags));
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
    var userUpdateDto = new UserUpdateDto(1, new HashSet<string>(){"Docker", "Framework", "dotnet"});

    var update = await _repository.Update(userUpdateDto);

    var option = await _repository.Read(1);

    var expected = new UserDto(1, "Jalle", new HashSet<string>(){ "Docker", "Framework", "dotnet" });

    Assert.Equal(Updated, update);
    Assert.Equal(expected.Id, option.Value.Id);
    Assert.Equal(expected.Name, option.Value.Name);
    Assert.True(option.Value.Tags.SetEquals(expected.Tags));
}

[Fact]
public async Task UpdateGivenNonExistingIdReturnNotFound()
{

    var userUpdateDto = new UserUpdateDto(3, new HashSet<string>());

    var status = await _repository.Update(userUpdateDto);

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


 private bool UsersEquals(IEnumerable<UserDto> users, IEnumerable<UserDto> others)
    {
        if (users.Count() != others.Count())
        {
            return false;
        }

        var uList = users.OrderBy(u => u.Id).ToList();
        var oList = others.OrderBy(u => u.Id).ToList();
        others.GetEnumerator().MoveNext();
        for (int i = 0; i < uList.Count(); i++)
        {
            if (!UserEquals(uList[i], oList[i]))
            {
                return false;
            }
        }

        return true;
    }

    private bool UserEquals(UserDto user, UserDto other)
    {
        if (user.Id != other.Id && user.Name != other.Name)
        {
            return false;
        }

        // Magic sauce to check that two enumerables have identical contents.
        // https://stackoverflow.com/questions/4576723/test-whether-two-ienumerablet-have-the-same-values-with-the-same-frequencies
        var tags = user.Tags.ToLookup(t => t);
        var otherTags = other.Tags.ToLookup(t => t);
        return tags.Count() == otherTags.Count()
            && tags.All(g => g.Count() == otherTags[g.Key].Count());
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