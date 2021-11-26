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
        IQueryable<Material> result = QueryTitle(options.SearchString);

        if (options.Tags != null)
        {
            return await QueryTags(options.Tags).ToListAsync();
        }
        
        if (options.StartDate != null)
        {
            result = QueryStartDate(result, (DateTime) options.StartDate);
        }
        
        if (options.EndDate != null)
        {
            result = QueryEndDate(result, (DateTime) options.EndDate);
        }

        return await result
            .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))))
            .ToListAsync();
    }

    private IQueryable<Material> QueryTitle(IQueryable<Material> source, string title) => source.Where(m => m.Title.Contains(title));
    
    private IQueryable<Material> QueryTitle(string title) => QueryTitle(_context.Materials, title);

    private IQueryable<MaterialDto> QueryTags(IEnumerable<string> tags)
    {
        return _context.Tags
            .Where(t => tags.Contains(t.Name))
            .SelectMany(t => t.Materials)
            .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));
    }

    private IQueryable<Material> QueryStartDate(DateTime start) => QueryStartDate(_context.Materials, start);

    private IQueryable<Material> QueryStartDate(IQueryable<Material> source, DateTime start) => source.Where(m => m.Date >= start);

    private IQueryable<Material> QueryEndDate(DateTime end) => QueryEndDate(_context.Materials, end);

    private IQueryable<Material> QueryEndDate(IQueryable<Material> source, DateTime end) => source.Where(m => m.Date <= end);

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}