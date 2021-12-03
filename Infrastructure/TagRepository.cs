namespace blueberry.Infrastructure;
//Class made with inspiration from Rasmus Lystrøm's repository: https://github.com/ondfisk/BDSA2021/blob/main/MyApp.Infrastructure/CityRepository.cs

public class TagRepository : ITagRepository
{
    private readonly IBlueberryContext _context;

    public TagRepository(IBlueberryContext context)
    {
        _context = context;
    }

    public async Task<(Status, TagDto)> Create(TagCreateDto tag)
    {
        var conflict = await _context.Tags
                        .Where(t => t.Name == tag.Name)
                        .Select(t => new TagDto(t.Id, tag.Name))
                        .FirstOrDefaultAsync();

        if (conflict != null)
        {
            return (Conflict, conflict);
        }

        var entity = new Tag{ Name = tag.Name };
        
        _context.Tags.Add(entity);

        await _context.SaveChangesAsync();

        return (Created, new TagDto(entity.Id, entity.Name));
    }

    public async Task<Status> Delete(int id)
    {
        var entity = await _context.Tags
                            .Include(t => t.Users)
                            .FirstOrDefaultAsync(t => t.Id == id);

        if (entity == null)
        {
            return NotFound;
        }

        if (entity.Users.Any())
        {
            return Conflict;
        }

        _context.Tags.Remove(entity);
        await _context.SaveChangesAsync();

        return Deleted;
    }

    public async Task<Option<TagDto>> Read(int id)
    {
        var tag = from t in _context.Tags
                            where t.Id == id
                            select new TagDto(t.Id, t.Name);

            return await tag.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<TagDto>> Read()
    {
        var tags = (await _context.Tags
                    .Select(t => new TagDto(t.Id, t.Name))
                    .ToListAsync())
                    .AsReadOnly();

        return tags;
    }
}