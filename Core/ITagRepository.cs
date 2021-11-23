namespace blueberry.Core;

public interface ITagRepository
{
    public Task<(Status, TagDto)> Create(TagCreateDto tag);
    public Task<Option<TagDto>> Read(int id);
    public Task<IReadOnlyCollection<TagDto>> Read();
    public Task<Status> Delete(int id);
}   