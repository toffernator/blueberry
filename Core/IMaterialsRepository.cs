namespace blueberry.Core;

public interface IMaterialRepository : IDisposable
{
    public Task<IReadOnlyCollection<MaterialDto>> Search(IQueryable<MaterialDto> criteria);
}