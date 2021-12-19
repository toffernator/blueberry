namespace blueberry.Core;

public record MaterialDto(int Id, string Title, PrimitiveCollection<string> Tags, string? ImageUrl, string Type, DateTime Date, string ShortDescription);
