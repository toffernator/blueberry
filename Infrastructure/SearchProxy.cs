namespace blueberry.Infrastructure;

public class SearchProxy : ISearch
{
    private readonly ISearch _search;
    private readonly CacheMap<SearchOptions, IReadOnlyCollection<MaterialDto>> _optionsCache;
    private readonly CacheMap<string, IReadOnlyCollection<MaterialDto>> _stringCache;

    public SearchProxy(IMaterialRepository repo)
    {
        _search = new SearchProvider(repo);
        _optionsCache = new CacheMap<SearchOptions, IReadOnlyCollection<MaterialDto>>(10);
        _stringCache = new CacheMap<string, IReadOnlyCollection<MaterialDto>>(10);
    }

    public async Task<IReadOnlyCollection<MaterialDto>> Search(string searchString)
    {
        if (searchString == null) throw new ArgumentException("Cannot search for null");
        if (_stringCache.TryGetValue(searchString, out IReadOnlyCollection<MaterialDto>? cached))
            return cached;
        var result = await _search.Search(searchString);
        _stringCache.Add(searchString, result);
        return result;
    }

    public async Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options)
    {
        if (options == null) throw new ArgumentException("Cannot search for null");
        if (_optionsCache.TryGetValue(options, out IReadOnlyCollection<MaterialDto>? cached))
            return cached;
        var result = await _search.Search(options);
        _optionsCache.Add(options, result);
        return result;
    }
}