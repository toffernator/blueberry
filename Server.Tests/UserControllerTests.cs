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
        // FIXME: Throws NotImplementedException
        // var response = await controller.Get(1);
        var exception = await Record.ExceptionAsync(() =>
            controller.Get(1)
        );


        // Assert
        // FIXME: Use this once NotImplemetedException is no longer thrown
        // Assert.Equal(user, response.Value);
        Assert.NotNull(exception);
        Assert.IsType<NotImplementedException>(exception);
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
        // FIXME: Throws NotImplementedException
        // var response = await controller.UpdateTags(1, tags);
        var exception = await Record.ExceptionAsync(() => 
            controller.UpdateTags(1, tags)
        );

        // Assert
        // FIXME: Use this once NotImplemetedException is no longer thrown
        // Assert.IsType<NoContentResult>(response);
        Assert.NotNull(exception);
        Assert.IsType<NotImplementedException>(exception);
    }
}
