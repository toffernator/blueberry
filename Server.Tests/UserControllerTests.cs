namespace blueberry.Server.Tests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task Get_given_existing_returns_User()
    {
        // Arrange
        var logger = new Mock<ILogger<UserController>>();
        var repository = new Mock<IUserRepository>();
        var user = new UserDto(1, "Rasmus", new HashSet<string>());
        repository.Setup(m => m.Read(1)).ReturnsAsync(user);
        var controller = new UserController(logger.Object, repository.Object);

        // Act
        var response = await controller.Get(1);

        // Assert
        Assert.Equal(user, response.Value);
    }

    [Fact]
    public async Task UpdateTags_updates_User()
    {
        // Arrange
        var logger = new Mock<ILogger<UserController>>();
        var repository = new Mock<IUserRepository>();
        var tags = new HashSet<string>();
        var userUpdate = new UserUpdateDto(1, tags);
        repository.Setup(m => m.Update(userUpdate)).ReturnsAsync(Updated);
        var controller = new UserController(logger.Object, repository.Object);

        // Act
        var response = await controller.UpdateTags(1, tags);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }
}
