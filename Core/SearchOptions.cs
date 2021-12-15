namespace blueberry.Core;

public record SearchOptions(string SearchString = "", PrimitiveSet<string>? Tags = null, string? Type = null, DateTime? StartDate = null, DateTime? EndDate = null, int? Limit = null, int? Offset = null, Sortings? SortBy = null);
