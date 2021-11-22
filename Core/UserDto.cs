namespace blueberry.Core;

public record UserDto(int Id, string Name, IReadOnlySet<string> Interests);
public record UserCreateDto(string Name, IReadOnlySet<string> Interests);
public record UserUpdateDto(int Id, string Name, IReadOnlySet<string> Interests) : UserCreateDto(Name, Interests);