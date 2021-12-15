namespace blueberry.Server.Tests.Controllers;


public class UserControllerTests
{

[   Fact]
    public async Task PostCreateUser()
    {
        // Arrange
        var logger = new Mock<ILogger<UserController>>();
        var repository = new Mock<IUserRepository>();
        var tags = new PrimitiveCollection<string>();
        var userToPost = new UserCreateDto("Jalle", tags);
        repository.Setup(u => u.Create(It.IsAny<UserCreateDto>())).ReturnsAsync(new UserDto(1, "Jalle", tags));

        // Act
        var controller = new UserController(logger.Object, repository.Object);
        var created = await controller.Post(userToPost);

        // Assert
        Assert.Equal(new UserDto(1, "Jalle", tags), created.Value);
    }

    [Fact]
    public async Task GetReturnUsersFromRepo()
    {
        // Arrange
        var logger = new Mock<ILogger<UserController>>();
        var repository = new Mock<IUserRepository>();
        var users = Array.Empty<UserDto>();
        repository.Setup(m => m.Read()).ReturnsAsync(users);
        var controller = new UserController(logger.Object, repository.Object);

        // Act
        var result = await controller.Get();

        // Assert
        Assert.Equal(users, result);
    }

    [Fact]
    public async Task GetGivenNonExistingUserReturnNotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<UserController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.Read(777)).ReturnsAsync(default(UserDto));
        var controller = new UserController(logger.Object, repository.Object);

        // Act
        var result = await controller.Get(777);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetGivenExistingUserReturnUser()
    {
        // Arrange
        var logger = new Mock<ILogger<UserController>>();
        var repository = new Mock<IUserRepository>();
        var user = new UserDto(1, "Jalle", new PrimitiveCollection<string>());
        repository.Setup(m => m.Read(1)).ReturnsAsync(user);
        var controller = new UserController(logger.Object, repository.Object);

        // Act
        var result = await controller.Get(1);

        // Assert
        Assert.Equal(user, result.Value);
    }


    [Fact]
    public async Task PutUpdatesUser()
    {
        // Arrange
        var logger = new Mock<ILogger<UserController>>();
        var repository = new Mock<IUserRepository>();
        var tags = new PrimitiveCollection<string>();
        var userUpdate = new UserUpdateDto(1, tags);
        repository.Setup(m => m.Update(1, userUpdate)).ReturnsAsync(Updated);
        var controller = new UserController(logger.Object, repository.Object);

        // Act
        var result = await controller.Put(1, userUpdate);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }


    [Fact]
    public async Task PutGivenNonExistingIdReturnNotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<UserController>>();
        var repository = new Mock<IUserRepository>();
        var tags = new PrimitiveCollection<string>();
        var userUpdate = new UserUpdateDto(1, tags);
        repository.Setup(m => m.Update(1, userUpdate)).ReturnsAsync(NotFound);
        var controller = new UserController(logger.Object, repository.Object);

        // Act
        var result = await controller.Put(1, userUpdate);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteGivenNonExistingUserReturnNotFound()
    {
        // Arrange
        var logger = new Mock<ILogger<UserController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.Delete(777)).ReturnsAsync(NotFound);
        var controller = new UserController(logger.Object, repository.Object);

        // Act
        var result = await controller.Delete(777);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteGivenExistingUserReturnNoContent()
    {
        // Arrange
        var logger = new Mock<ILogger<UserController>>();
        var repository = new Mock<IUserRepository>();
        repository.Setup(m => m.Delete(777)).ReturnsAsync(Deleted);
        var controller = new UserController(logger.Object, repository.Object);

        // Act
        var result = await controller.Delete(777);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
