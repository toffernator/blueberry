namespace blueberry.Server.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class MaterialController : ControllerBase
{
    private readonly ILogger<MaterialController> _logger;
    private readonly ISearch _search;

    public MaterialController(ILogger<MaterialController> logger, ISearch search)
    {
        _logger = logger;
        _search = search;
    }

    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(IEnumerable<MaterialDto>), 200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaterialDto>>> Get([FromQuery(Name = "searchString")] string? searchString,
                                                                  [FromQuery(Name = "tag")] string? tags,
                                                                  [FromQuery(Name = "startyear")] int? startYear,
                                                                  [FromQuery(Name = "endyear")] int? endYear,
                                                                  [FromQuery(Name = "type")] string? type,
                                                                  [FromQuery(Name = "offset")] int offset,
                                                                  [FromQuery(Name = "limit")] int limit,
                                                                  [FromQuery(Name = "sortby")] string? sortByQueryParam,
                                                                  [FromQuery(Name = "userid")] int userid)
    {
        var decodedTags = tags == null ? null : new PrimitiveSet<string>(Uri.UnescapeDataString(tags).Split(","));
        var enumParseSuccess = Enum.TryParse(sortByQueryParam, out Sortings sortBy);

        var options = new SearchOptions
        {
            SearchString = Uri.UnescapeDataString(searchString ?? ""),
            Tags = decodedTags,
            StartDate = startYear is null ? null : new DateTime((int)startYear, 1, 1),
            EndDate = endYear is null ? null : new DateTime((int)endYear, 12, 31),
            Type = type is null ? "" : type,
            Limit = limit,
            Offset = offset,
            SortBy = enumParseSuccess ? sortBy : null
        };

        var result = await _search.Search(options, userid);
        return new ActionResult<IEnumerable<MaterialDto>>(result);
    }
}
