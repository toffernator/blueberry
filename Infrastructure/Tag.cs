namespace blueberry.Infrastructure;

public class Tag
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<User> Users { get; set; } = new PrimitiveCollection<User>();
    public ICollection<Material> Materials { get; set; } = new PrimitiveCollection<Material>();

    public Tag(string name)
    {
        if (name == null)
        {
            throw new ArgumentException("All non-nullable arguments must be non-null");
        }
        Name = name;
    }
}