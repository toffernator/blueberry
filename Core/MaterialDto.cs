namespace blueberry.Core;

public record MaterialDto(int id, string title, ICollection<Tag> tags);
public record MaterialDetailsDto(int id, string title, ICollection<Tag> tags, string shortDescription, string? imageUrl, MediaType Type, DateTime date) : MaterialDto(id, title, tags);