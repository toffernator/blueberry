namespace blueberry.Server.Tests.Controllers;

public class MaterialControllerTests
{

    [Theory]
    [InlineData(0, 2)]
    [InlineData(0, 1)]
    [InlineData(2, 1)]
    public async Task GetGivenOptionsOffsetAndLimitReturnsRangeOfResults(int offset, int limit)
    {
        var logger = new Mock<ILogger<MaterialController>>();
        var repository = new Mock<IMaterialRepository>();
        
       var expected = new [] 
        {
            new MaterialDto(1, "Lecture 10", new [] {"Docker", "C#"}),
            new MaterialDto(2, "Lecture 16", new [] {"Docker", "C#"})
        };
        repository.Setup(m => m.Search(It.IsAny<SearchOptions>())).ReturnsAsync(expected);
        
        var controller = new MaterialController(logger.Object, repository.Object);
        var response = await controller.Get("Lecture", new HashSet<string> {"Docker"}, 2021, 2022, "Video", offset, limit);

        var isEqual = MaterialsEquals(expected.Skip(offset).Take(limit), response.Value);
        Assert.True(isEqual);
    }
}
