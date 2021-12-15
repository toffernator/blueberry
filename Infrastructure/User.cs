namespace blueberry.Infrastructure;

public class User
{
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; }

    public ICollection<Tag> Tags { get; set; } = new PrimitiveCollection<Tag>();

    public User(string name)
    {
        if (name == null)
        {
            throw new ArgumentException("All non-nullable arguments must be non-null");
        }
        Name = name;
    }
}