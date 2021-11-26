namespace blueberry.Infrastructure;

public class MaterialRepository : IMaterialRepository
{
    private readonly IBlueberryContext _context;

    public MaterialRepository(IBlueberryContext context)
    {
        _context = context;
    }
    public async Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options)
    {
        if (options.searchString != "")
        {
            return await QueryTitle(options.searchString).ToListAsync();
        }
        else if (options.tags != null)
        {
            return await QueryTag(options.tags.First()).ToListAsync();
        }


        return new HashSet<MaterialDto>() ;
    }

    private IQueryable<MaterialDto> QueryTitle(string title)
    {
        return _context.Materials
            .Where(m => m.Title == title)
            .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));
    }

    private IQueryable<MaterialDto> QueryTag(string tag)
    {
        // Find the exact tag that refers to this tag. Assume that there is only one per name, since name is UNIQUE.
        var tagEntity = _context.Tags.Where(t => t.Name == tag).First();

        var materials = _context.Tags
            .Where(t => t.Name == tag)
            .SelectMany(t => t.Materials);

        return materials.Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));

        // return _context.Materials
        //     .Where(m => m.Tags.Contains(tagEntity))
        //     .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}