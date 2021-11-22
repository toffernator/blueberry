namespace blueberry.Core;

public interface ITagRepository
{
    public Task<TagDto> Create(TagCreateDto tag);
    public Task<Option<TagDto>> Read(int id);
    public Task<Status> Delete(int id);
}   