namespace blueberry.Infrastructure;

public class MaterialRepository : IMaterialRepository
{
    private readonly IBlueberryContext _context;

    public MaterialRepository(IBlueberryContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}