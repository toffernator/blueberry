namespace blueberry.Infrastructure;

public class SearchProvider : ISearch
{
    private readonly IMaterialRepository _repo;
    private readonly IUserRepository _users;

    public SearchProvider(IMaterialRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _users = userRepo;
    }

    public async Task<IReadOnlyCollection<MaterialDto>> Search(string searchString)
    {
        var searchOptions = new SearchOptions(searchString, null, null, null);
        return await Search(searchOptions);
    }

    public async Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options)
    {
        return await _repo.Search(options);
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
            return await _repo.Search(newOptions);
        }
        return await _repo.Search(options);
    }

    private bool searchEmpty(SearchOptions options)
    {
        return options.SearchString == ""
               && options.Type == ""
               && options.StartDate == null
               && options.EndDate == null
               && options.Tags ==  null;
    }
}
