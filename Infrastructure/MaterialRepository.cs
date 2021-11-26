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
            return await QueryTags(options.tags).ToListAsync();
        }

        return new HashSet<MaterialDto>() ;
    }

    private IQueryable<MaterialDto> QueryTitle(string title)
    {
        return _context.Materials
            .Where(m => m.Title == title)
            .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));
    }

    private IQueryable<MaterialDto> QueryTags(IEnumerable<string> tags)
    {
        return _context.Tags
            .Where(t => tags.Contains(t.Name))
            .SelectMany(t => t.Materials)
            .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}