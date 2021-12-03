namespace blueberry.Server.Model;
//Taken from Rasmus Lystrøm's repository : https://github.com/ondfisk/BDSA2021/blob/main/MyApp.Server/Model/Extensions.cs
public static class Extensions
{
    public static IActionResult ToActionResult(this Status status) => status switch
    {
        Status.Updated => new NoContentResult(),
        Status.Deleted => new NoContentResult(),
        Status.NotFound => new NotFoundResult(),
        Status.Conflict => new ConflictResult(),
        _ => throw new NotSupportedException($"{status} not supported")
    };

    public static ActionResult<T> ToActionResult<T>(this Option<T> option) where T : class
        => option.IsSome ? option.Value : new NotFoundResult();
}