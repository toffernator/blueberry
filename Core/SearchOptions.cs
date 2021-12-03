namespace blueberry.Core;

public record SearchOptions(string SearchString = "", HashSet<string>? Tags = null, string? Type = null, DateTime? StartDate = null, DateTime? EndDate = null);
