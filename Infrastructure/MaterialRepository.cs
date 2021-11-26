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
        else if (options.startDate != null && options.endDate != null)
        {
            return await QueryBoundedDate((DateTime) options.startDate, (DateTime) options.endDate).ToListAsync();
        }
        else if (options.startDate != null)
        {
            return await QueryStartDate((DateTime) options.startDate).ToListAsync();
        }
        else if (options.endDate != null)
        {
            return await QueryEndDate((DateTime) options.endDate).ToListAsync();
        }

        return new HashSet<MaterialDto>();
    }

    private IQueryable<MaterialDto> QueryTitle(string title)
    {
        return _context.Materials
            .Where(m => m.Title.Contains(title))
            .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));
    }

    private IQueryable<MaterialDto> QueryTags(IEnumerable<string> tags)
    {
        return _context.Tags
            .Where(t => tags.Contains(t.Name))
            .SelectMany(t => t.Materials)
            .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));
    }

    private IQueryable<MaterialDto> QueryStartDate(DateTime start)
    {
        return _context.Materials
            .Where(m => m.Date >= start)
            .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));
    }

    private IQueryable<MaterialDto> QueryEndDate(DateTime end)
    {
        return _context.Materials
            .Where(m => m.Date <= end)
            .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));
    }

    private IQueryable<MaterialDto> QueryBoundedDate(DateTime start, DateTime end)
    {
        return _context.Materials
            .Where(m => m.Date >= start && m.Date <= end)
            .Select(m => new MaterialDto(m.Id, m.Title, new HashSet<string>(m.Tags.Select(t => t.Name))));
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}