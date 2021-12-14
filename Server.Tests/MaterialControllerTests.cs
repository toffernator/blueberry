namespace blueberry.Server.Tests.Controllers;

public class MaterialControllerTests
{
    [Fact]
    public async Task GetCreatesCorectSearchOptionsFromParameters()
    {
        var logger = new Mock<ILogger<MaterialController>>();
        var repository = new Mock<IMaterialRepository>();

        var mockOptions = new SearchOptions
        {
            SearchString = "Lecture",
            Tags = new PrimitiveSet<string>() { "Docker" },
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2022, 12, 31),
            Type = "Video",
            Limit = 2,
            Offset = 0
        };
        var expected = new[]
        {
            new MaterialDto(1, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}),
            new MaterialDto(2, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"})
        };
        repository.Setup(m => m.Search(It.Is<SearchOptions>(o => o.Equals(mockOptions)))).ReturnsAsync(expected);

        Console.WriteLine(mockOptions);

        var controller = new MaterialController(logger.Object, repository.Object);
        var response = await controller.Get("Lecture", new HashSet<string> { "Docker" }, 2021, 2022, "Video", 0, 2);

        Assert.Equal(expected, response.Value);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(0, 1)]
    [InlineData(2, 1)]
    public async Task GetGivenOptionsOffsetAndLimitReturnsRangeOfResults(int offset, int limit)
    {
        var logger = new Mock<ILogger<MaterialController>>();
        var repository = new Mock<IMaterialRepository>();

        var expected = new[]
         {
            new MaterialDto(1, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}),
            new MaterialDto(2, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"})
        };
        repository.Setup(m => m.Search(It.IsAny<SearchOptions>())).ReturnsAsync(expected.Skip(offset).Take(limit).ToList());

        var controller = new MaterialController(logger.Object, repository.Object);
        var response = await controller.Get("Lecture", new HashSet<string> { "Docker" }, 2021, 2022, "Video", offset, limit);


        Assert.Equal(expected.Skip(offset).Take(limit), response.Value);

        //var isEqual = MaterialsEquals(expected.Skip(offset).Take(limit), response.Value);
        //Assert.True(isEqual);
    }
}
