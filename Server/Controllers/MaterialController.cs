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

    [HttpGet]
    public IEnumerable<MaterialDTO> Get([FromQuery(Name = "tag")] string? tag, [FromQuery(Name = "keywords")] string? keywords,
                     [FromQuery(Name = "startyear")] int? startYear, [FromQuery(Name = "endyear")] int? endYear)
    {
        Console.WriteLine(String.Format("Tags = {0}\n Keywords = {1}\n Years = {2} - {3}", tag, keywords, startYear, endYear));
        throw new NotImplementedException();
    }
}
