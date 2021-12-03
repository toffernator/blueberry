namespace blueberry.Core;

public record UserDto(int Id, string Name, PrimitiveCollection<string> Tags);
public record UserCreateDto(string Name, PrimitiveCollection<string> Tags);
public record UserUpdateDto(int Id, PrimitiveCollection<string> Tags);
