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
        Console.WriteLine($"MaterialRepository/Search | Found {_context.Materials.Count()} materials");

        // Start with title since it is the only non-nullable option.
        IQueryable<Material> result = QueryTitle(options.SearchString);
        Console.WriteLine($"MaterialRepository/Search/SearchString | Found {result.Count()} materials");

        if (options.Tags != null && options.Tags.Count() != 0)
        {
            Console.WriteLine($"MaterialRepository/Search/Tags | Has Tags: ");
            foreach(string t in options.Tags)
            {
                Console.Write(t + ", ");
            }
            Console.WriteLine();

            result = QueryTags(result, options.Tags);
            Console.WriteLine($"MaterialRepository/Search/Tags | Found {result.Count()} materials");
        }

        if (options.StartDate != null)
        {
            result = QueryStartDate(result, (DateTime)options.StartDate);
            Console.WriteLine($"MaterialRepository/Search/StartDate | Found {result.Count()} materials");
            // FIXME: Here the count becomes 0
        }

        if (options.EndDate != null)
        {
            result = QueryEndDate(result, (DateTime)options.EndDate);
            Console.WriteLine($"MaterialRepository/Search/EndDate | Found {result.Count()} materials");
        }

        if (options.Type != null && options.Type != "")
        {
            result = QueryType(result, options.Type);
            Console.WriteLine($"MaterialRepository/Search/Type | Found {result.Count()} materials");
        }

        Console.WriteLine($"MaterialRepository/Search | Found {result.Count()} result");

        return await result
            .Select(m => new MaterialDto(m.Id, m.Title, new PrimitiveCollection<string>(m.Tags.Select(t => t.Name))))
            .ToListAsync();
    }

    private IQueryable<Material> QueryTitle(IQueryable<Material> source, string title) => source.Where(m => m.Title.ToUpper().Contains(title.ToUpper()));
    
    private IQueryable<Material> QueryTitle(string title) => QueryTitle(_context.Materials, title);

    private IQueryable<Material> QueryTags(IEnumerable<string> tags) => QueryTags(_context.Materials, tags);

    private IQueryable<Material> QueryTags(IQueryable<Material> source, IEnumerable<string> tags)
    {
        var tagEntities = _context.Tags.Where(t => tags.Contains(t.Name));
        Console.WriteLine($"MaterialRepository/Search/Tags | Has Tags: ");
        foreach(Tag t in tagEntities)
        {
            Console.Write(t + ", ");
        }
        Console.WriteLine();

        return source.Where(m => m.Tags.Intersect(tagEntities).Count() >= 0);
    }

    private IQueryable<Material> QueryStartDate(DateTime start) => QueryStartDate(_context.Materials, start);

    private IQueryable<Material> QueryStartDate(IQueryable<Material> source, DateTime start) => source.Where(m => m.Date.Date >= start.Date);

    private IQueryable<Material> QueryEndDate(DateTime end) => QueryEndDate(_context.Materials, end);

    private IQueryable<Material> QueryEndDate(IQueryable<Material> source, DateTime end) => source.Where(m => m.Date.Date <= end.Date);

    private IQueryable<Material> QueryType(string type) => QueryType(_context.Materials, type);
    private IQueryable<Material> QueryType(IQueryable<Material> source, string type) => source.Where(m => m.Type == type);
}