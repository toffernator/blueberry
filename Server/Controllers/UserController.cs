using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using blueberry.Core;

namespace blueberry.Server.Controllers;

//[Authorize]
[ApiController]
[Route("[controller]")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id}")]
    public UserDTO Get(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id}")]
    public UserDTO UpdateTags(int id, [FromBody] IEnumerable<TagDTO> tags)
    {
        throw new NotImplementedException();
    }
   
}
