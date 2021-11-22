namespace blueberry.Core;

public record MaterialDto(int Id, string Title, ICollection<TagDto> Tags);
public record MaterialDetailsDto(int Id, string Title, ICollection<TagDto> Tags, string ShortDescription, string? ImageUrl, MediaTypeDto Type, DateTime Date) : MaterialDto(Id, Title, Tags);
