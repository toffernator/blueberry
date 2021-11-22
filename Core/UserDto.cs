namespace blueberry.Core;

public record UserDto(int id, string name, ICollection<Tag> tags);
public record UserCreateDto(string name, ICollection<Tag> tags);
public record UserUpdateDto(int id, string name, ICollection<Tag> tags) : UserCreateDto(name, tags);