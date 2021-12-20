namespace blueberry.Core;

/// <summary>Defines the interface for searching through a body of materials</summary>
public interface ISearch
{
    /// <summary>Search for materials matching searchString</summary>
    public Task<IReadOnlyCollection<MaterialDto>> Search(string searchString);

    /// <summary>Search for materials matching the options specified in the <c>SearchOptions</c> object given as <param>options</param>.</summary>
    public Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options);

    /// <summary>Search by <c>SearchOptions</c> and a user id, interests being fetched for the user</summary>
    public Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options, int id);
}
