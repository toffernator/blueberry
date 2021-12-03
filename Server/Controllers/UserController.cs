namespace blueberry.Server.Controllers;
//Made with inspiration from Rasmus Repo : https://github.com/ondfisk/BDSA2021/blob/main/MyApp.Server/Controllers/CharactersController.cs

[ApiController]
[Route("[controller]")]
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
    public async Task<IActionResult> Post(UserCreateDto character)
    {
        var created = await _repository.Create(character);
        return CreatedAtRoute(nameof(Get), new { created.Id }, created);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
        => (await _repository.Delete(id)).ToActionResult();
}
