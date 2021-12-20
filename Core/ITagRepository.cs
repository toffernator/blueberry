namespace blueberry.Core;

/// <summary>Defines the interface for manipulating a body of <c>Tag</c>s.
public interface ITagRepository
{
    /// <summary>Creates a new <c>Tag</c> in the repository.</summary>
    /// <returns>A status, and the newly created <c>Tag</c> if <c>Status == Status.Created</c></returns>
    public Task<(Status, TagDto)> Create(TagCreateDto tag);
    
    /// <summary>Reads a <c>Tag</c> with the specified <c>id</c></summary>
    /// <returns>A <c>TagDto</c> if the <c>Tag</c> was found, <c>null</c> otherwise.</returns>
    public Task<Option<TagDto>> Read(int id);

    /// <summary>Reads all <c>Tag</c>s in the repository.</summary>
    /// <returns>A collection of <c>TagDto</c> containing all tags in the repository. May be empty, but not <c>null</c>.</returns>
    public Task<IReadOnlyCollection<TagDto>> Read();

    /// <summary>Deletes a <c>Tag</c> with the specified <c>id</c>.</summary>
    /// <returns><c>Status.Deleted</c> if the <c>Tag</c> was deleted, <c>Status.NotFound</c> if it was not found.</returns>
    public Task<Status> Delete(int id);
}   