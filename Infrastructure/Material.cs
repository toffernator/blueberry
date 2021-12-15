namespace blueberry.Infrastructure;

public class Material
{
    public int Id { get; init; }

    [StringLength(50)]
    public string Title { get; init; }

    [StringLength(250)]
    public string ShortDescription { get; init; }

    public ICollection<Tag> Tags { get; init; } = new PrimitiveCollection<Tag>();

    [StringLength(250)]
    [Url]
    public string? ImageUrl { get; init; }

    public string Type { get; init; }

    public DateTime Date { get; init; }

    public Material(string title, string shortDescription, string type, DateTime date)
    {
        if (title == null || shortDescription == null || type == null)
        {
            throw new ArgumentException("Make sure to set all non-nullable properties");
        }
        Title = title;
        ShortDescription = shortDescription;
        Type = type;
        Date = date;
    }
}