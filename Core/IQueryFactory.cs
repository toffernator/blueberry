namespace blueberry.Core;

public interface IQueryFactory
{
    public IQueryable<MaterialDetailsDto> Create(IEnumerable<string?> tags, string? keywords, DateTime? startDate, DateTime? endDate);
}