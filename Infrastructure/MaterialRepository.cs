namespace blueberry.Infrastructure;

public class MaterialRepository : IMaterialRepository
{
    private readonly IBlueberryContext _context;

    public MaterialRepository(IBlueberryContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<MaterialDto>> Search(IQueryable<MaterialDto> criteria)
    {
        throw new NotImplementedException();
    }
}