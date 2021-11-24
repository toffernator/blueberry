namespace blueberry.Core;

public interface ISearch
{
    public Task<IReadOnlyCollection<MaterialDto>> Search(string searchString);
    public Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options);
}
