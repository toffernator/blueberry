namespace blueberry.Server.Controllers;
//Made with inspiration from Rasmus Repo : https://github.com/ondfisk/BDSA2021/blob/main/MyApp.Server/Controllers/CharactersController.cs

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepository _repository;

    public UserController(ILogger<UserController> logger, IUserRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<UserDto>> Get()
        //Deliberately doesn't return an ActionResult as an empty collection of users is still a valid case. No Users registered yet.
        => await _repository.Read();

    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(UserDto), 200)]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> Get(int id)
        => (await _repository.Read(id)).ToActionResult(); 

    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Put(int id, [FromBody] UserUpdateDto user)
        => (await _repository.Update(id, user)).ToActionResult(); 

    [HttpPost]
    [ProducesResponseType(typeof(UserDto), 201)]
    public async Task<ActionResult<UserDto>> Post(UserCreateDto user)
        => (await _repository.Create(user));

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
        => (await _repository.Delete(id)).ToActionResult();
}
