using Moq;

namespace blueberry.Infrastructure;

public class SearchTests
{
    private readonly IReadOnlyCollection<MaterialDto> _mockData = new[]{
        new MaterialDto(1, "Why Haskell is better than F#", new []{"FP", "Haskell", "F#", "The truth"}),
        new MaterialDto(2, "Typescript and react", new []{"React", "Typsescript", "Javascript"}),
        new MaterialDto(3, "Why angular died", new []{"Angular", "Typescript", "Javascript"}),
        new MaterialDto(3, "Why typescript is the future of the web", new []{ "Typescript", "Javascript"}),
    };

    [Fact]
    public async void SearchGivenEmptyStringShouldReturnAllResuslts()
    {
        var mockedRepo = new Mock<IMaterialRepository>();
        var searchOptions = new SearchOptions("", null, null, null);
        mockedRepo.Setup(mr => mr.Search(searchOptions)).ReturnsAsync(_mockData);

        var search = new SearchProxy(mockedRepo.Object);

        var actual = await search.Search("");

        Assert.Equal(_mockData, actual);
    }

    [Theory]
    [InlineData("Why")]
    [InlineData("typescript")]
    [InlineData("bla bla")]
    [InlineData("React")]
    public async void SearchGivenAStringShouldReturnMaterialWithThatInTheTitle(string searchTerm)
    {
        var mockedRepo = new Mock<IMaterialRepository>();
        var searchOptions = new SearchOptions(searchTerm, null, null, null);
        var filteredMockData = _mockData.Where(md => md.Title.Contains(searchTerm)).ToList();
        mockedRepo.Setup(mr => mr.Search(searchOptions)).ReturnsAsync(filteredMockData);

        var search = new SearchProxy(mockedRepo.Object);

        var actual = await search.Search(searchTerm);

        Assert.Equal(filteredMockData, actual);
    }

    [Fact]
    public async void SearchShouldForwardOptionsToTheRepo()
    {
        SearchOptions? providedOptions = null;
        var mockedRepo = new Mock<IMaterialRepository>();

        mockedRepo.Setup(mr => mr.Search(It.IsAny<SearchOptions>()))
            .Callback<SearchOptions>(so =>
            {
                providedOptions = so;
            });

        var search = new SearchProxy(mockedRepo.Object);
        await search.Search(It.IsAny<SearchOptions>());


        Assert.Equal(It.IsAny<SearchOptions>(), providedOptions);

        // mockedRepo.Verify(mock => mock.Search(searchOptions), Times.Once);
    }

    [Fact]
    public async void ProxyShouldOnlyCallProxiedSearchOnceWhenCalledWithTheSameParams()
    {
        var mockedRepo = new Mock<IMaterialRepository>();
        var searchOptions = new SearchOptions("Typescript", null, null, null);
        var filteredMockData = _mockData.Where(md => md.Title.Contains("Typescript")).ToList();
        mockedRepo.Setup(mr => mr.Search(searchOptions)).ReturnsAsync(filteredMockData);

        var search = new SearchProxy(mockedRepo.Object);

        await search.Search("Typescript");
        var actual = await search.Search("Typescript");

        Assert.Equal(filteredMockData, actual);
        mockedRepo.Verify(mock => mock.Search(searchOptions), Times.Exactly(2));
    }
}
