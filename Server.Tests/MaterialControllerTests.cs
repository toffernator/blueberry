namespace blueberry.Server.Tests.Controllers;

public class MaterialControllerTests
{
    [Fact]
    public async Task GetCreatesCorrectSearchOptionsFromParameters()
    {
        var logger = new Mock<ILogger<MaterialController>>();
        var search = new Mock<ISearch>();

        var mockOptions = new SearchOptions
        {
            SearchString = "Lecture",
            Tags = new PrimitiveSet<string>() { "Docker" },
            StartDate = new DateTime(2021, 1, 1),
            EndDate = new DateTime(2022, 12, 31),
            Type = "Video",
            Limit = 2,
            Offset = 0,
            SortBy = Sortings.NEWEST
        };
        var testUserId = 1;

        var expected = new[]
        {
            new MaterialDto(1, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, null, "Video", DateTime.Today),
            new MaterialDto(2, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, null, "Video", DateTime.Today)
        };
        search.Setup(m => m.Search(It.Is<SearchOptions>(o => o.Equals(mockOptions)), It.Is<int>(id => id.Equals(testUserId)))).ReturnsAsync(expected);

        Console.WriteLine(mockOptions);

        var controller = new MaterialController(logger.Object, search.Object);
        var response = await controller.Get("Lecture", "Docker", 2021, 2022, "Video", 0, 2, Sortings.NEWEST.ToString(), 1);

        Assert.Equal(expected, response.Value);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(0, 1)]
    [InlineData(2, 1)]
    public async Task GetGivenOptionsOffsetAndLimitReturnsRangeOfResults(int offset, int limit)
    {
        var logger = new Mock<ILogger<MaterialController>>();
        var search = new Mock<ISearch>();

        var testUserId = 1;
        var expected = new[]
         {
            new MaterialDto(1, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, null, "Video", DateTime.Today),
            new MaterialDto(2, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, null, "Video", DateTime.Today)
        };
        search.Setup(m => m.Search(It.IsAny<SearchOptions>(), It.Is<int>(id => id.Equals(testUserId)))).ReturnsAsync(expected.Skip(offset).Take(limit).ToList());

        var controller = new MaterialController(logger.Object, search.Object);
        var response = await controller.Get("Lecture", "Docker", 2021, 2022, "Video", offset, limit, Sortings.NEWEST.ToString(),1);

        Assert.Equal(expected.Skip(offset).Take(limit), response.Value);
    }
}
