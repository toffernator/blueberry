namespace blueberry.Infrastructure;

public class Tag
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
    public ICollection<Material> Materials { get; set; }
}