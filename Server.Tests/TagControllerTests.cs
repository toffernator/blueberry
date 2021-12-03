namespace blueberry.Server.Tests.Controllers;

public class TagControllerTests {
    [Fact]
    public async Task Get_returns_Tags_from_repo()
    {
        // Arrange
        var logger = new Mock<ILogger<TagController>>();
        var expected = new TagDto[] {
            new TagDto(1, "Docker"),
            new TagDto(2, "React"),
        };
        var repository = new Mock<ITagRepository>();
        repository.Setup(m => m.Read()).ReturnsAsync(expected);
        var controller = new TagController(logger.Object, repository.Object);

        // Act
        var actual = await controller.Get();
        
        // Assert
        Assert.Equal(expected, actual);
    }
}
