namespace blueberry.Infrastructure;

public class SearchProvider : ISearch
{
    private readonly IMaterialRepository _repo;

    public SearchProvider(IMaterialRepository repo)
    {
        _repo = repo;
    }

    public Task<IReadOnlyCollection<MaterialDto>> Search(string searchString)
    {
        var searchOptions = new SearchOptions(searchString, null, null, null);
        return Search(searchOptions);
    }

    public Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options)
    {
        return _repo.Search(options);
    }
}
