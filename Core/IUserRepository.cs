namespace blueberry.Core;

public interface IUserRepository
{
    public Task<UserDto> Create(UserCreateDto user);
    public Task<Option<UserDto>> Read(int id);
    public Task<IReadOnlyCollection<UserDto>> Read();
    public Task<Status> Update(UserUpdateDto user);
    public Task<Status> Delete(int id);
}