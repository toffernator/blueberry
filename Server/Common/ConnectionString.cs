namespace blueberry.Server.Common;

public class ConnectionString
{
    public static string Read(params string?[] maybeStrings)
    {
        foreach (var maybe in maybeStrings)
        {
            if (maybe != null && maybe.Length > 0)
            {
                return maybe;
            }
        }
        throw new ArgumentException("No connection string supplied");
    }
}