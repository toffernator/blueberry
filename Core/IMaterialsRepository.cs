namespace blueberry.Core;

public interface IMaterialRepository
{
    public Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options); 
}