namespace blueberry.Infrastructure;

public class MaterialRepository : IMaterialRepository
{
    private readonly IBlueberryContext _context;

    public MaterialRepository(IBlueberryContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<MaterialDto>> Search(IQueryable<MaterialDto> criteria)
    {
        IReadOnlyCollection<MaterialDto> result = await criteria.ToListAsync();
        return result;
    }
}