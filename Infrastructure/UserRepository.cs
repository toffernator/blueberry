namespace blueberry.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly IBlueberryContext _context;

    public UserRepository(IBlueberryContext context)
    {
        _context = context;
    }

    public async Task<UserDto> Create(UserCreateDto User)
    {
        var entity = new User
        {
            Name = User.Name,
            Interests = await GetInterests(User.Interests).ToListAsync()
        };

        _context.Users.Add(entity);

        await _context.SaveChangesAsync();

        return new UserDto(
                        entity.Id,
                        entity.Name,
                        entity.Interests.Select(u => u.Name).ToHashSet()
                    );
    }

    public async Task<Option<UserDto>> Read(int userId)
    {
        var users = from u in _context.Users
                    where u.Id == userId
                    select new UserDto(
                        u.Id,
                        u.Name,
                        u.Interests.Select(u => u.Name).ToHashSet()
                    );

        return await users.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<UserDto>> Read() => 
        (await _context.Users
                    .Select(u => new UserDto(u.Id, u.Name, u.Interests.Select(u => u.Name).ToHashSet()))
                    .ToListAsync())
                    .AsReadOnly();

    public async Task<Status> Update(UserUpdateDto User)
    {
        var entity = await _context.Users.Include(u => u.Interests).FirstOrDefaultAsync(u => u.Id == User.Id);

        if(entity == null)
        {
            return NotFound;
        }

        entity.Interests = await GetInterests(User.Interests).ToListAsync();

        await _context.SaveChangesAsync();

        return Updated;
    }

    public async Task<Status> Delete(int userId)
    {
        var entity = await _context.Users.FindAsync(userId);

        if(entity == null)
        {
            return NotFound;
        }

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();

        return Deleted;
    }

    private async IAsyncEnumerable<Tag> GetInterests(IEnumerable<string> interests)
    {
        var existing = await _context.Tags.Where(i => interests.Contains(i.Name)).ToDictionaryAsync(i => i.Name);

        foreach (var tag in interests)
        {
            yield return existing.TryGetValue(tag, out var i) ? i : new Tag(tag);
        }

    }
}
