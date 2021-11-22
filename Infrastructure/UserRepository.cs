namespace blueberry.Infrastructure;

public class UserRepository : IUserRepository
{


    public async Task<UserDetailsDto> CreateAsync(UserCreateDto user)
    {
        var entity = new User
        {
            
        }
    }
}