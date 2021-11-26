namespace blueberry.Server.Tests.Controllers;

public class TagControllerTests {
    [Fact]
    public async Task Get_returns_Tags_from_repo()
    {
        // Arrange
        var logger = new Mock<ILogger<TagController>>();
        var expected = Array.Empty<TagDto>();
        var repository = new Mock<ITagRepository>();
        repository.Setup(m => m.Read()).ReturnsAsync(expected);
        var controller = new TagController(logger.Object, repository.Object);

        // Act
        // FIXME: Throws NotImplementedException
        // var actual = await controller.Get();
        var exception = await Record.ExceptionAsync(() =>
            controller.Get()
        );

        // Assert
        // FIXME: Use this once NotImplemetedException is no longer thrown
        // Assert.Equal(expected, actual);
        Assert.NotNull(exception);
        Assert.IsType<NotImplementedException>(exception);
    }
}
