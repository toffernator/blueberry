namespace blueberry.Infrastructure;

public class SearchProxy : ISearch
{
    private readonly ISearch _search;
    private readonly IUserRepository _users;
    private readonly CacheMap<SearchOptions, IReadOnlyCollection<MaterialDto>> _optionsCache;
    private readonly CacheMap<string, IReadOnlyCollection<MaterialDto>> _stringCache;

    public SearchProxy(IMaterialRepository searchProvider, IUserRepository userRepo)
    {
        _search = new SearchProvider(searchProvider);
        _users = userRepo;
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

    public async Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options, int id)
    {
        if(searchEmpty(options))
        {
            var user = await _users.Read(id);
            var interests = new PrimitiveSet<string>();

            if(user.IsSome)
            {
                interests = new PrimitiveSet<string>(user.Value.Tags);
            }

            var newOptions = new SearchOptions { SearchString = options.SearchString , Tags = interests, Type = options.Type,
                                                            StartDate = options.StartDate, EndDate = options.EndDate };
            return await _search.Search(newOptions);
        }

        return await _search.Search(options);
    }

    private bool searchEmpty(SearchOptions options)
    {
        if(options.SearchString == "" && options.Type == "" && options.StartDate == null
                                        && options.EndDate == null && options.Tags ==  null)
        {
            return true;
        }
        return false;
    }

}
