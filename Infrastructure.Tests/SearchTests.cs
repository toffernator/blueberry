using Moq;

namespace blueberry.Infrastructure;

public class SearchTests
{
    private readonly IReadOnlyCollection<MaterialDto> _mockMaterials = new[]
    {
        new MaterialDto(1, "Why Haskell is better than F#", new PrimitiveCollection<string> {"FP", "Haskell", "F#", "The truth"}),
        new MaterialDto(2, "Typescript and react", new PrimitiveCollection<string> {"React", "Typsescript", "Javascript"}),
        new MaterialDto(3, "Why angular died", new PrimitiveCollection<string> {"Angular", "Typescript", "Javascript"}),
        new MaterialDto(4, "Why typescript is the future of the web", new PrimitiveCollection<string> { "Typescript", "Javascript"}),
    };

    private readonly IReadOnlyCollection<UserDto> _mockUsers = new[]
    {
        new UserDto (1, "Jalle", new PrimitiveCollection<string> {"React"}),
        new UserDto (2, "Kobo", new PrimitiveCollection<string> {"Angular"})
    };

    [Fact]
    public async void SearchGivenEmptyStringShouldReturnAllResuslts()
    {
        var mockedMaterialRepo = new Mock<IMaterialRepository>();
        var mockedUserRepo = new Mock<IUserRepository>();
        var searchOptions = new SearchOptions("", null, null, null);
        mockedMaterialRepo.Setup(mr => mr.Search(searchOptions)).ReturnsAsync(_mockMaterials);

        var search = new SearchProxy(mockedMaterialRepo.Object, mockedUserRepo.Object);

        var actual = await search.Search("");

        Assert.Equal(_mockMaterials, actual);
    }

    [Theory]
    [InlineData("Why")]
    [InlineData("typescript")]
    [InlineData("bla bla")]
    [InlineData("React")]
    public async void SearchGivenAStringShouldReturnMaterialWithThatInTheTitle(string searchTerm)
    {
        var mockedMaterialRepo = new Mock<IMaterialRepository>();
        var mockedUserRepo = new Mock<IUserRepository>();
        var searchOptions = new SearchOptions(searchTerm, null, null, null);
        var filteredMockData = _mockMaterials.Where(md => md.Title.Contains(searchTerm)).ToList();
        mockedMaterialRepo.Setup(mr => mr.Search(searchOptions)).ReturnsAsync(filteredMockData);

        var search = new SearchProxy(mockedMaterialRepo.Object, mockedUserRepo.Object);

        var actual = await search.Search(searchTerm);

        Assert.Equal(filteredMockData, actual);
    }

    [Fact]
    public async void SearchShouldForwardOptionsToTheRepo()
    {
        SearchOptions? receivedOptions = null;
        SearchOptions givenOptions = new SearchOptions();
        var mockedRepo = new Mock<IMaterialRepository>();
        var mockedUserRepo = new Mock<IUserRepository>();

        mockedRepo.Setup(mr => mr.Search(It.IsAny<SearchOptions>()))
            .Callback<SearchOptions>(so =>
            {
                receivedOptions = so;
            });

        var search = new SearchProxy(mockedRepo.Object, mockedUserRepo.Object);
        await search.Search(givenOptions);

        Assert.Equal(givenOptions, receivedOptions);
    }

    [Fact]
    public async void ProxyShouldOnlyCallProxiedSearchOnceWhenCalledWithTheSameParams()
    {
        var mockedMaterialRepo = new Mock<IMaterialRepository>();
        var mockedUserRepo = new Mock<IUserRepository>();
        var searchOptions = new SearchOptions("Typescript", null, null, null);
        var filteredMockData = _mockMaterials.Where(md => md.Title.Contains("Typescript")).ToList();
        mockedMaterialRepo.Setup(mr => mr.Search(searchOptions)).ReturnsAsync(filteredMockData);

        var search = new SearchProxy(mockedMaterialRepo.Object, mockedUserRepo.Object);

        await search.Search("Typescript");
        var actual = await search.Search("Typescript");

        Assert.Equal(filteredMockData, actual);
        mockedMaterialRepo.Verify(mock => mock.Search(searchOptions), Times.Exactly(1));
    }

    [Fact]
    public async void SearchWithUserIdAndSearchOptionsReturnsFilteredMaterialsBasedOnSearch()
    {
        var mockedMaterialRepo = new Mock<IMaterialRepository>();
        var mockedUserRepo = new Mock<IUserRepository>();

        var mockSearchOptions = new SearchOptions("", new PrimitiveSet<string>(){"Typescript"}, null, null);

        var searchOptions = new SearchOptions("",new PrimitiveSet<string>(){"Typescript"},null,null);
        
        var filteredMockData = _mockMaterials.Where(md => md.Tags.Contains("Typescript")).ToList();
        var readUser = _mockUsers.Where(u => u.Id == 1).FirstOrDefault();
        
        mockedMaterialRepo.Setup( mr => mr.Search(mockSearchOptions) ).ReturnsAsync(filteredMockData);

        mockedUserRepo.Setup( u => u.Read(1)).ReturnsAsync(readUser);

        var search = new SearchProxy(mockedMaterialRepo.Object, mockedUserRepo.Object);

        var actual = await search.Search(searchOptions, 1);

        Assert.Equal(filteredMockData, actual);  
    }

    [Fact]
    public async void SearchWithUserIdAndNoSearchOptionsReturnSearchBasedOnInterests()
    {
        var mockedMaterialRepo = new Mock<IMaterialRepository>();
        var mockedUserRepo = new Mock<IUserRepository>();

        var mockSearchOptions = new SearchOptions("", null, null, null);
        var searchOptions = new SearchOptions("", null, null, null);
        
        var filteredMockData = _mockMaterials.Where(md => md.Id != 1).ToList();
        var readUser = _mockUsers.Where(u => u.Id == 2).FirstOrDefault();
        
        mockedMaterialRepo.Setup( mr => mr.Search(mockSearchOptions) ).ReturnsAsync(filteredMockData);

        mockedUserRepo.Setup( u => u.Read(2)).ReturnsAsync(readUser);


        var search = new SearchProxy(mockedMaterialRepo.Object, mockedUserRepo.Object);

        var actual = await search.Search(searchOptions, 2);
        
        Assert.Equal(filteredMockData, actual);  
    }
}
