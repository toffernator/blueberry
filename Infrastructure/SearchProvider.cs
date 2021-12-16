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
        if (options.Tags == null)
        {
            var user = await _users.Read(id);

            var interests = new PrimitiveSet<string>();
            if (user.IsSome)
            {
                interests = new PrimitiveSet<string>(user.Value.Tags);
            }
            
            var optionsWithUserInteresets = new SearchOptions
            { 
                SearchString = options.SearchString,
                Tags = interests,
                Type = options.Type,
                StartDate = options.StartDate,
                EndDate = options.EndDate,
                Limit = options.Limit,
                Offset = options.Offset,
                SortBy = options.SortBy
            };

            return await _repo.Search(optionsWithUserInteresets);
        }

        return await _repo.Search(options);
    }
}
