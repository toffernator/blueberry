namespace blueberry.Core;

public record UserDto(int Id, string Name, ICollection<TagDto> Tags);
public record UserCreateDto(string Name, ICollection<TagDto> Tags);
public record UserUpdateDto(int Id, string Name, ICollection<TagDto> Tags) : UserCreateDto(Name, Tags);