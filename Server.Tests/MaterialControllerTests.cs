namespace blueberry.Server.Tests.Controllers;

public class MaterialControllerTests
{
    [Fact]
    public async Task Get_given_null_returns_all_Materials()
    {
        // Arrange
        var logger = new Mock<ILogger<MaterialController>>();
        var repository = new Mock<IMaterialRepository>();
        var expected = Array.Empty<MaterialDto>(); 
        repository.Setup(m => m.Read()).ReturnsAsync(expected);
        var controller = new MaterialController(logger.Object, repository.Object);

        // Act
        var response = await controller.Get(null, null, null, null);

        // Assert
        Assert.Equal(expected, response.Value);
    }

}
