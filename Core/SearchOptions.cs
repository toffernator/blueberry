namespace blueberry.Core;

public record SearchOptions(string searchString, ISet<string>? tags, DateTime? startDate, DateTime? endDate);