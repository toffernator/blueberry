using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using blueberry.Core;

namespace blueberry.Server.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class TagController : ControllerBase
{
    private readonly ILogger<TagController> _logger;

    public TagController(ILogger<TagController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<TagDTO> Get()
    {
        throw new NotImplementedException();
    }
}
