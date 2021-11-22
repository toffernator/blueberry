namespace blueberry.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly IBlueberryContext _context;

    public UserRepoistory(IBlueberryContext context)
    {
        _context = context;
    }

    public async Task<UserDto> CreateAsync(UserCreateDto User)
    {
        var entity = new User
        {
            Name = User.Name,
            Interests = User.Interests
        };

        _context.Users.Add(entity);

        await _context.SaveChangesAsync();

        return new UserDto(
                        entity.Id,
                        entity.Name,
                        entity.Interests
                    );
    }

    public async Task<Option<UserDto>> ReadAsync(int userId)
    {
        var users = from u in _context.Users
                    where u.Id == userId
                    select new UserDto(
                        u.Id,
                        u.Name,
                        u.Interests.Select(u => u.Name).ToHashSet()
                    );
    
    }
}