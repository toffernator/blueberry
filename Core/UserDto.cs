namespace blueberry.Core;

public record UserDto(int Id, string Name, IReadOnlySet<string> Tags);
public record UserCreateDto(string Name, IReadOnlySet<string> Tags);
public record UserUpdateDto(int Id, IReadOnlySet<string> Tags);
