namespace blueberry.Core;

/// <summary>Defines the interface for manipulating a body of <c>Material</c>s.
public interface IMaterialRepository
{
    /// <summary>Search through this repository for <c>Material</c>s matching the given <c>SearchOptions</c>.</summary>
    /// <param name="options">A <c>SearchOptions</c> object defining the criteria for the search</c></param>
    /// <returns>A collection of all <c>MaterialDto</c>s matching the criteria</returns>
    public Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options);
}