namespace blueberry.Core;

/// <summary>Defines the interface for manipulating a body of <c>User</c>s.
public interface IUserRepository
{
    /// <summary>Creates a new <c>User</c> in the repository.</summary>
    /// <param name="user">A <c>UserCreateDto</c> object describing the new <c>User</c> object.</param>
    /// <returns>The newly created <c>User</c></returns>
    public Task<UserDto> Create(UserCreateDto user);

    /// <summary>Reads a <c>User</c> with the specified <c>id</c></summary>
    /// <param name="id">The ID if the <c>User</c> object to read</param>
    /// <returns>A <c>UserDto</c> if the <c>User</c> was found, <c>null</c> otherwise.</returns>
    public Task<Option<UserDto>> Read(int id);

    /// <summary>Reads all <c>User</c>s in the repository.</summary>
    /// <returns>A collection of <c>UserDto</c> containing all tags in the repository. May be empty, but not <c>null</c>.</returns>
    public Task<IReadOnlyCollection<UserDto>> Read();

    /// <summary>Updates the <c>User</c> entity which has the given <c>id</c>.</summary>
    /// <param name="id">The ID if the <c>User</c> object to alter</param>
    /// <param name="user">A <c>UserUpdateDto</c> object describing the change to be made to the <c>User</c> object.</param>
    public Task<Status> Update(int id, UserUpdateDto user);

    /// <summary>Deletes a <c>User</c> with the specified <c>id</c>.</summary>
    /// <param name="id">The ID if the <c>User</c> object to delete</param>
    /// <returns><c>Status.Deleted</c> if the <c>User</c> was deleted, <c>Status.NotFound</c> if it was not found.</returns>
    public Task<Status> Delete(int id);
}