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
        // Start with title since it is the only non-nullable option.
        IQueryable<Material> result = QueryTitle(options.SearchString);

        if (options.Tags != null && options.Tags.Count() != 0)
        {
            result = QueryTags(result, options.Tags);
        }

        if (options.StartDate != null)
        {
            result = QueryStartDate(result, (DateTime)options.StartDate);
        }

        if (options.EndDate != null)
        {
            result = QueryEndDate(result, (DateTime)options.EndDate);
        }

        if (options.Type != null && options.Type != "")
        {
            result = QueryType(result, options.Type);
        }

        result = QueryOrdering(result, options.SortBy != null ? options.SortBy.Value : Sortings.NEWEST);

        if (options.Offset != null)
        {
            result = QueryOffset(result, options.Offset.Value);
        }

        result = result.Take(options.Limit ?? 30);

        return await result
            .Select(m => new MaterialDto(m.Id, m.Title, new PrimitiveCollection<string>(m.Tags.Select(t => t.Name)), m.ImageUrl, m.Type, m.Date, m.ShortDescription))
            .ToListAsync();
    }

    private IQueryable<Material> QueryTitle(IQueryable<Material> source, string title) => source.Where(m => m.Title.ToUpper().Contains(title.ToUpper()));

    private IQueryable<Material> QueryTitle(string title) => QueryTitle(_context.Materials, title);

    private IQueryable<Material> QueryTags(IEnumerable<string> tags) => QueryTags(_context.Materials, tags);

    private IQueryable<Material> QueryTags(IQueryable<Material> source, IEnumerable<string> tags)
    {
        var tagEntities = _context.Tags.Where(t => tags.Contains(t.Name));
        // First check that the tags actually exist. Otherwise, the intersection with the empty set is always the empty
        // set.
        if (tagEntities.Count() == 0)
        {
            return source;
        }
        return source.Where(m => m.Tags.Intersect(tagEntities).Count() > 0);
    }

    private IQueryable<Material> QueryStartDate(DateTime start) => QueryStartDate(_context.Materials, start);

    private IQueryable<Material> QueryStartDate(IQueryable<Material> source, DateTime start) => source.Where(m => m.Date >= start);

    private IQueryable<Material> QueryEndDate(DateTime end) => QueryEndDate(_context.Materials, end);

    private IQueryable<Material> QueryEndDate(IQueryable<Material> source, DateTime end) => source.Where(m => m.Date <= end);

    private IQueryable<Material> QueryType(string type) => QueryType(_context.Materials, type);
    private IQueryable<Material> QueryType(IQueryable<Material> source, string type) => source.Where(m => m.Type == type);
    private IQueryable<Material> QueryOffset(IQueryable<Material> source, int offset) => source.Skip(offset);
    private IQueryable<Material> QueryOrdering(IQueryable<Material> source, Sortings sortBy)
    {
        switch (sortBy)
        {
            case Sortings.AZ:
                return source.OrderBy(m => m.Title);
            case Sortings.ZA:
                return source.OrderByDescending(m => m.Title);
            case Sortings.OLDEST:
                return source.OrderBy(m => m.Date);
            case Sortings.NEWEST:
            default:
                return source.OrderByDescending(m => m.Date);
        }
    }
}