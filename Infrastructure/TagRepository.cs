namespace blueberry.Infrastructure;

public class TagRepository : ITagRepository
{
    private readonly IBlueberryContext _context;

    public TagRepository(IBlueberryContext context)
    {
        _context = context;
    }

    public Task<(Status, TagDto)> Create(TagCreateDto tag)
    {
        throw new NotImplementedException();
    }

    public Task<Status> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Option<TagDto>> Read(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<TagDto>> Read()
    {
        throw new NotImplementedException();
    }

    Task<(Status, TagDto)> ITagRepository.Create(TagCreateDto tag)
    {
        throw new NotImplementedException();
    }
}