using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using blueberry.Core;

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
    [ProducesResponseType(typeof(IEnumerable<MaterialDTO>), 200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaterialDTO>>> Get([FromQuery(Name = "tag")] string? tag, [FromQuery(Name = "keywords")] string? keywords,
                                                                  [FromQuery(Name = "startyear")] int? startYear, [FromQuery(Name = "endyear")] int? endYear)
    {
        throw new NotImplementedException();
    }
}
