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
        return await QueryTitle(options.searchString).ToListAsync();
    }

    private IQueryable<MaterialDto> QueryTitle(string title)
    {
        return _context.Materials
            .Where(m => m.Title == title)
            .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}