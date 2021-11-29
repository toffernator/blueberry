namespace blueberry.Core;
//Class from Rasmus Lyngstrøm's repository -> https://github.com/ondfisk/BDSA2021/blob/d2700e19b999f937132363cfcd310c806cb88a09/MyApp.Core/Extensions.cs#L10

public static class Extensions
{
    public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items,
            CancellationToken cancellationToken = default)
        {
            var results = new List<T>();
            await foreach (var item in items.WithCancellation(cancellationToken)
                                            .ConfigureAwait(false))
                results.Add(item);
            return results;
        }
}