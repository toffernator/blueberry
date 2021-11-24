using System.Collections.Generic;

namespace blueberry.Core;

public interface IMaterialRepository : IDisposable
{
    public Task<IReadOnlyCollection<MaterialDto>> Search(SearchOptions options); 
}