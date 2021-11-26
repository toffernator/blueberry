namespace blueberry.Server.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class TagController : ControllerBase
{
    private readonly ILogger<TagController> _logger;
    private readonly ITagRepository _repository;



    public TagController(ILogger<TagController> logger, ITagRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<TagDto>> Get()
    {
        throw new NotImplementedException();
    }
}
