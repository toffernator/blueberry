namespace blueberry.Server.Tests.Controllers;

public class MaterialControllerTests
{

    [Fact]
    public async Task GetGivenLectureTagDockerStartYear2021EndYear2022TypeVideoOffset0Limit2ReturnsLecture10AndLecture16()
    {
        var logger = new Mock<ILogger<MaterialController>>();
        var repository = new Mock<IMaterialRepository>();
        
        var mockOptions = new SearchOptions
        {
            SearchString = "Lecture",
            Tags = new HashSet<string>() {"Docker"},
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2022, 1, 1),
            Type = "Video"
        };
        var expected = new [] 
        {
            new MaterialDto(1, "Lecture 10", new [] {"Docker", "C#"}),
            new MaterialDto(2, "Lecture 16", new [] {"Docker", "C#"})
        };
        repository.Setup(m => m.Search(It.IsAny<SearchOptions>())).ReturnsAsync(expected);
    
    

        
        var controller = new MaterialController(logger.Object, repository.Object);
        var response = await controller.Get("Lecture", new HashSet<string> {"Docker"}, 2021, 2022, "Video", 0, 2);

        var isEqual = MaterialsEquals(expected, response.Value);
        Assert.True(isEqual);
    }

}
