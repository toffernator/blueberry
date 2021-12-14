namespace blueberry.Infrastructure;

public class SearchProxy : ISearch
{
    private readonly ISearch _search;
    private readonly IUserRepository _users;

    public SearchProxy(IMaterialRepository searchProvider, IUserRepository userRepo)
    {
        _search = new SearchProvider(searchProvider);
        _users = userRepo;
    }

    public Task<IReadOnlyCollection<MaterialDto>> Search(string searchString)
    {
        return _search.Search(searchString);
    }

    public Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options)
    {
        return _search.Search(options);
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
