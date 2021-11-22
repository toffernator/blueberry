namespace blueberry.Core;

public interface IUserRepository
{
    public Task<UserDto> Create(UserCreateDto user);
    public Task<UserDto> Read(int id);
    public Task<Status> Update(UserUpdateDto user);
    public Task<Status> Delete(int id);
}