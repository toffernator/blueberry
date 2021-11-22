namespace blueberry.Core;

public record MaterialDto(int Id, string Title, IReadOnlySet<string> Tags);
public record MaterialDetailsDto(int Id, string Title, IReadOnlySet<string> Tags, string ShortDescription, string? ImageUrl, string Type, DateTime Date) : MaterialDto(Id, Title, Tags);
