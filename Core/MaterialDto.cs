namespace blueberry.Core;

public record MaterialDto(int Id, string Title, PrimitiveCollection<string> Tags, string? ImageUrl, string Type, DateTime Date);
public record MaterialDetailsDto(int Id, string Title, PrimitiveCollection<string> Tags, string ShortDescription, string? ImageUrl, string Type, DateTime Date) : MaterialDto(Id, Title, Tags, ImageUrl, Type, Date);
