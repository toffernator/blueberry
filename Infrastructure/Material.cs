namespace blueberry.Infrastructure;

public class Material
{
    public int Id { get; set; }

    [StringLength(50)]
    public string Title { get; set; }

    [StringLength(250)]
    public string ShortDescription { get; set; }

    public ICollection<Tag> Tags { get; set; }

    [StringLength(250)]
    [Url]
    public string? ImageUrl { get; set; }

    public string Type { get; set; }

    public DateTime Date { get; set; }
}