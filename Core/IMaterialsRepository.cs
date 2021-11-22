namespace blueberry.Core;

public interface IMaterialRepository
{
    public Task<IReadOnlyCollection<MaterialDetailsDto>> Search(IQueryable<MaterialDetailsDto> criteria);
}