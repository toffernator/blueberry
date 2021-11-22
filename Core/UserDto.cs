namespace blueberry.Core;

public record UserDto(int Id, string Name, ICollection<TagDto> Interest);
public record UserCreateDto(string Name, ICollection<TagDto> Interests);
public record UserUpdateDto(int Id, string Name, ICollection<TagDto> Interests) : UserCreateDto(Name, Interests);