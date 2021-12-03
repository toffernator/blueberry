namespace blueberry.Infrastructure;

public class SearchProxy : ISearch
{
    private readonly ISearch _search;

    public SearchProxy(IMaterialRepository repo)
    {
        _search = new SearchProvider(repo);
    }

    public Task<IReadOnlyCollection<MaterialDto>> Search(string searchString)
    {
        return _search.Search(searchString);
    }

    public Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options)
    {
        return _search.Search(options);
    }
}