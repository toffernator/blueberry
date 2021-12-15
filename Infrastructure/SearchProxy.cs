namespace blueberry.Infrastructure;

public class SearchProxy : ISearch
{
    private readonly ISearch _search;
    private readonly CacheMap<SearchOptions, IReadOnlyCollection<MaterialDto>> _optionsCache;
    private readonly CacheMap<string, IReadOnlyCollection<MaterialDto>> _stringCache;
    private readonly CacheMap<(SearchOptions, int), IReadOnlyCollection<MaterialDto>> _optionsIdCache;

    public SearchProxy(IMaterialRepository materialRepo, IUserRepository userRepo)
    {
        _search = new SearchProvider(materialRepo, userRepo);
        _optionsCache = new CacheMap<SearchOptions, IReadOnlyCollection<MaterialDto>>(10);
        _optionsIdCache = new CacheMap<(SearchOptions, int), IReadOnlyCollection<MaterialDto>>(10);
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

    public async Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options, int id)
    {
        if (options == null) throw new ArgumentException("Cannot search for null");
        if (_optionsIdCache.TryGetValue((options, id), out IReadOnlyCollection<MaterialDto>? cached))
            return cached;
        var result = await _search.Search(options, id);
        _optionsIdCache.Add((options, id), result);
        return result;
    }
}
