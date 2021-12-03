namespace blueberry.Server.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class MaterialController : ControllerBase
{
    private readonly ILogger<MaterialController> _logger;
    private readonly IMaterialRepository _repository;

    public MaterialController(ILogger<MaterialController> logger, IMaterialRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(IEnumerable<MaterialDto>), 200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaterialDto>>> Get([FromQuery(Name = "searchString")] string? searchString, 
                                                                  [FromQuery(Name = "tag")] HashSet<string> tags,
                                                                  [FromQuery(Name = "startyear")] int? startYear, 
                                                                  [FromQuery(Name = "endyear")] int? endYear, 
                                                                  [FromQuery(Name = "type")] string? type,
                                                                  [FromQuery(Name = "offset")] int offset, 
                                                                  [FromQuery(Name = "limit")] int limit)
    {
        var options = new SearchOptions
        {
            SearchString = searchString is null ? "" : searchString,
            Tags = tags is null ? new HashSet<string>() : tags,
            StartDate = startYear is null ? null : new DateTime((int) startYear, 1, 1),
            EndDate = endYear is null ? null : new DateTime((int) endYear, 1, 1),
            Type = type is null ? "" : type
        };

        var result = await _repository.Search(options);
        return new ActionResult<IEnumerable<MaterialDto>>(result.Skip(offset).Take(limit));
    }
}
