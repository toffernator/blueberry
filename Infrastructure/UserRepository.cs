namespace blueberry.Infrastructure;
//Class made with inspiration from Rasmus Lystrøm's repository: https://github.com/ondfisk/BDSA2021/blob/main/MyApp.Infrastructure/CharacterRepository.cs

public class UserRepository : IUserRepository
{
    private readonly IBlueberryContext _context;

    public UserRepository(IBlueberryContext context)
    {
        _context = context;
    }

    public async Task<UserDto> Create(UserCreateDto User)
    {
        var entity = new User(name: User.Name) { Tags = await GetTags(User.Tags).ToListAsync() };

        _context.Users.Add(entity);

        await _context.SaveChangesAsync();

        return new UserDto(entity.Id, entity.Name, new PrimitiveCollection<string>(entity.Tags.Select(u => u.Name)));
    }

    public async Task<Option<UserDto>> Read(int userId)
    {
        var users = from u in _context.Users
                    where u.Id == userId
                    select new UserDto(
                        u.Id,
                        u.Name,
                        new PrimitiveCollection<string>(u.Tags.Select(u => u.Name))
                    );

        return await users.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<UserDto>> Read() =>
        (await _context.Users
                    .Select(u => new UserDto(u.Id, u.Name, new PrimitiveCollection<string>(u.Tags.Select(u => u.Name))))
                    .ToListAsync())
                    .AsReadOnly();

    public async Task<Status> Update(int id, UserUpdateDto User)
    {
        var entity = await _context.Users.Include(u => u.Tags).FirstOrDefaultAsync(u => u.Id == User.Id);

        if (entity == null)
        {
            return NotFound;
        }

        entity.Tags = await GetTags(User.Tags).ToListAsync();

        await _context.SaveChangesAsync();

        return Updated;
    }

    public async Task<Status> Delete(int userId)
    {
        var entity = await _context.Users.FindAsync(userId);

        if (entity == null)
        {
            return NotFound;
        }

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();

        return Deleted;
    }

    private async IAsyncEnumerable<Tag> GetTags(IEnumerable<string> tags)
    {
        var existing = await _context.Tags.Where(t => tags.Contains(t.Name)).ToDictionaryAsync(t => t.Name);

        foreach (var tag in tags)
        {
            yield return existing.TryGetValue(tag, out var t) ? t : new Tag(name: tag);
        }
    }
}
