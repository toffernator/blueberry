namespace blueberry.Infrastructure;

public class Material
{
    public int Id { get; set; }
    
    [StringLength(50)]
    public string title { get; set; }

    [StringLength(250)]
    public string shortDescription { get; set; }

    public ICollection<Tag> tags { get; set; }

    [StringLength(250)]
    [Url]
    public string imageUrl { get; set; }

    public MediaType type { get; set; }

    public DateTime date { get; set; }
}