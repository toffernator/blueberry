namespace blueberry.Core;

public record UserDto(int id, string name, ICollection<TagDto> tags);
public record UserCreateDto(string name, ICollection<TagDto> tags);
public record UserUpdateDto(int id, string name, ICollection<TagDto> tags) : UserCreateDto(name, tags);