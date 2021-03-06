namespace Infrastructure.Tests;

public class MaterialRepositoryTests : IDisposable
{
    private readonly BlueberryContext _context;
    private readonly MaterialRepository _repository;
    private bool disposedValue;

    public MaterialRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var builder = new DbContextOptionsBuilder<BlueberryContext>();
        builder.UseSqlite(connection);
        var context = new BlueberryContext(builder.Options);
        context.Database.EnsureCreated();

        var Docker = new Tag(name: "Docker");
        var Mobile = new Tag(name: "Mobile");
        var SE = new Tag(name: "Software Engineering");
        var CS = new Tag(name: "C#");

        context.Materials.AddRange(
            new Material(title: "OOSE", shortDescription: "", type: "Book", date: new DateTime(2013, 4, 20)) { Id = 1, Tags = new[] { SE }, ImageUrl = "" },
            new Material(title: "C# 9.0 in a Nutshell", shortDescription: "", type: "Book", date: new DateTime(2021, 2, 26)) { Id = 2, Tags = new[] { CS }, ImageUrl = "" },
            new Material(title: "Lecture 9", shortDescription: "", type: "Video", date: new DateTime(2021, 9, 29)) { Id = 9, Tags = new[] { SE }, ImageUrl = "" },
            new Material(title: "Lecture 10", shortDescription: "", type: "Video", date: new DateTime(2021, 10, 1)) { Id = 10, Tags = new[] { Docker, CS }, ImageUrl = "" },
            new Material(title: "Lecture 16", shortDescription: "", type: "Video", date: new DateTime(2021, 10, 29)) { Id = 16, Tags = new[] { Docker, CS }, ImageUrl = "" },
            new Material(title: "Lecture 20", shortDescription: "", type: "Video", date: new DateTime(2021, 11, 12)) { Id = 20, Tags = new[] { Mobile, CS }, ImageUrl = "" }
        );
        context.SaveChanges();

        _context = context;
        _repository = new MaterialRepository(_context);
    }

    [Fact]
    public async Task SearchGivenEmptyStringReturnsEverything()
    {
        var options = new SearchOptions { SearchString = "" };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new PrimitiveCollection<string> {"Software Engineering"},  "",  "Book",  new DateTime(2013, 4, 20), ""),
            new MaterialDto(2, "C# 9.0 in a Nutshell", new PrimitiveCollection<string> {"C#"}, "", "Book", new DateTime(2021, 2, 26), ""),
            new MaterialDto(9, "Lecture 9", new PrimitiveCollection<string> {"Software Engineering"}, "", "Video", new DateTime(2021, 9, 29), ""),
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), ""),
            new MaterialDto(20, "Lecture 20", new PrimitiveCollection<string> {"Mobile", "C#"}, "", "Video", new DateTime(2021, 11, 12), "")
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenEmptyTagCollectionReturnsEverything()
    {
        var options = new SearchOptions { Tags = new PrimitiveSet<string>() };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new PrimitiveCollection<string> {"Software Engineering"},  "",  "Book",  new DateTime(2013, 4, 20), ""),
            new MaterialDto(2, "C# 9.0 in a Nutshell", new PrimitiveCollection<string> {"C#"}, "", "Book", new DateTime(2021, 2, 26), ""),
            new MaterialDto(9, "Lecture 9", new PrimitiveCollection<string> {"Software Engineering"}, "", "Video", new DateTime(2021, 9, 29), ""),
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), ""),
            new MaterialDto(20, "Lecture 20", new PrimitiveCollection<string> {"Mobile", "C#"}, "", "Video", new DateTime(2021, 11, 12), "")
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenNonexistentTagsDoesNothing()
    {
        var options = new SearchOptions { Tags = new PrimitiveSet<string>() { "This tag never has, doesn't currently, and never will exist.!!!!ASDF" } };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new PrimitiveCollection<string> {"Software Engineering"},  "",  "Book",  new DateTime(2013, 4, 20), ""),
            new MaterialDto(2, "C# 9.0 in a Nutshell", new PrimitiveCollection<string> {"C#"}, "", "Book", new DateTime(2021, 2, 26), ""),
            new MaterialDto(9, "Lecture 9", new PrimitiveCollection<string> {"Software Engineering"}, "", "Video", new DateTime(2021, 9, 29), ""),
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), ""),
            new MaterialDto(20, "Lecture 20", new PrimitiveCollection<string> {"Mobile", "C#"}, "", "Video", new DateTime(2021, 11, 12), "")
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenLecture10ReturnsLecture10()
    {
        var options = new SearchOptions { SearchString = "Lecture 10" };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenLectureReturnsLecture9AndLecture10AndLecture16AndLecture20()
    {
        var options = new SearchOptions { SearchString = "Lecture" };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(9, "Lecture 9", new PrimitiveCollection<string> {"Software Engineering"}, "", "Video", new DateTime(2021, 9, 29), ""),
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), ""),
            new MaterialDto(20, "Lecture 20", new PrimitiveCollection<string> {"Mobile", "C#"}, "", "Video", new DateTime(2021, 11, 12), "")
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenTitleIgnoresCase()
    {
        var options = new SearchOptions { SearchString = "lEcTuRe 10" };
        var result = await _repository.Search(options);
        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenDockerTagReturnsLecture10AndLecture16()
    {
        var options = new SearchOptions { Tags = new PrimitiveSet<string>() { "Docker" } };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), "")
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenDockerAndMobileTagReturnsLecture10AndLecture16AndLecture20()
    {
        var options = new SearchOptions { Tags = new PrimitiveSet<string>() { "Docker", "Mobile" } };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), ""),
            new MaterialDto(20, "Lecture 20", new PrimitiveCollection<string> {"Mobile", "C#"}, "", "Video", new DateTime(2021, 11, 12), "")
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenStartDate29102021ReturnsLecture16AndLecture20()
    {
        var options = new SearchOptions { StartDate = new DateTime(2021, 10, 29) };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), ""),
            new MaterialDto(20, "Lecture 20", new PrimitiveCollection<string> {"Mobile", "C#"}, "", "Video", new DateTime(2021, 11, 12), "")
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenEndDate29102021ReturnsOOSEAndCS90AndLecture9Lecture10AndLecture16()
    {
        var options = new SearchOptions { EndDate = new DateTime(2021, 10, 29) };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new PrimitiveCollection<string> {"Software Engineering"},  "",  "Book",  new DateTime(2013, 4, 20), ""),
            new MaterialDto(2, "C# 9.0 in a Nutshell", new PrimitiveCollection<string> {"C#"}, "", "Book", new DateTime(2021, 2, 26), ""),
            new MaterialDto(9, "Lecture 9", new PrimitiveCollection<string> {"Software Engineering"}, "", "Video", new DateTime(2021, 9, 29), ""),
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), "")
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenTypeBookReturnsOOSEAndCS90()
    {
        var options = new SearchOptions { Type = "Book" };
        var result = await _repository.Search(options);
        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new PrimitiveCollection<string> {"Software Engineering"},  "",  "Book",  new DateTime(2013, 4, 20), ""),
            new MaterialDto(2, "C# 9.0 in a Nutshell", new PrimitiveCollection<string> {"C#"}, "", "Book", new DateTime(2021, 2, 26), ""),
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenStartDate29102021AndEndDate29102021ReturnsLecture16()
    {
        var options = new SearchOptions { StartDate = new DateTime(2021, 10, 29), EndDate = new DateTime(2021, 10, 29) };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), ""),
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGiven10AndDockerTagReturnsLecture10()
    {
        var options = new SearchOptions { SearchString = "10", Tags = new PrimitiveSet<string>() { "Docker" } };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGiven10AndStartDate29102021ReturnsNothing()
    {
        var options = new SearchOptions { SearchString = "10", StartDate = new DateTime(2021, 10, 29) };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>();

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGiven10AndEndDate29102021ReturnsLecture10()
    {
        var options = new SearchOptions { SearchString = "10", EndDate = new DateTime(2021, 10, 29) };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenLectureAndDockerAndStartDate29102021ReturnsLecture16()
    {
        var options = new SearchOptions { SearchString = "Lecture", Tags = new PrimitiveSet<string>() { "Docker" }, StartDate = new DateTime(2021, 10, 29) };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), "")
        };

        Assert.Equal(expected, result);
    }


    [Fact]
    public async Task SearchGivenDockerTagAndStartDate29102021ReturnsLecture16()
    {
        var options = new SearchOptions { Tags = new PrimitiveSet<string> { "Docker" }, StartDate = new DateTime(2021, 10, 29) };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenDockerTagAndEndDate29102021ReturnsLecture10AndLecture16()
    {
        var options = new SearchOptions { Tags = new PrimitiveSet<string> { "Docker" }, EndDate = new DateTime(2021, 10, 29) };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), "")
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenTypeBookAndLectureReturnsNothing()
    {
        var options = new SearchOptions { SearchString = "Lecture", Type = "Book" };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>();

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenTypeBookAndSoftwareEngineeringRetunsOOSE()
    {
        var options = new SearchOptions { Tags = new PrimitiveSet<string> { "Software Engineering" }, Type = "Book" };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new PrimitiveCollection<string> {"Software Engineering"},  "",  "Book",  new DateTime(2013, 4, 20), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenTypeVideoAndStartDate12112021ReturnsLecture20()
    {
        var options = new SearchOptions { StartDate = new DateTime(2021, 11, 12), Type = "Video" };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(20, "Lecture 20", new PrimitiveCollection<string> {"Mobile", "C#"}, "", "Video", new DateTime(2021, 11, 12), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenTypeVideoAndEndDate29102021ReturnsLecture10AndLecture16()
    {
        var options = new SearchOptions { EndDate = new DateTime(2021, 10, 29), Type = "Video" };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(9, "Lecture 9", new PrimitiveCollection<string> {"Software Engineering"}, "", "Video", new DateTime(2021, 9, 29), ""),
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), "")
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGiven10AndDockerAndStartDate01102021ReturnsLecture10()
    {
        var options = new SearchOptions
        {
            SearchString = "10",
            Tags = new PrimitiveSet<string>() { "Docker" },
            StartDate = new DateTime(2021, 10, 1)
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGiven16AndDockerAndEndDate29102021ReturnsLecture16()
    {
        var options = new SearchOptions
        {
            SearchString = "16",
            Tags = new PrimitiveSet<string>() { "Docker" },
            EndDate = new DateTime(2021, 10, 29)
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), ""),
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenOAndSoftwareEngineeringAndTypeBookReturnsOOSE()
    {
        var options = new SearchOptions
        {
            SearchString = "O",
            Tags = new PrimitiveSet<string>() { "Software Engineering" },
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new PrimitiveCollection<string> {"Software Engineering"},  "",  "Book",  new DateTime(2013, 4, 20), ""),
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenLectureAndStartDate01102021AndEndDate29102021ReturnsLecture10AndLecture16()
    {
        var options = new SearchOptions
        {
            SearchString = "Lecture",
            StartDate = new DateTime(2021, 10, 1),
            EndDate = new DateTime(2021, 10, 29)
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), ""),
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), ""),
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenOAndStartDate20042013AndTypeBookReturnsOOSE()
    {
        var options = new SearchOptions
        {
            SearchString = "O",
            StartDate = new DateTime(2013, 4, 20),
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new PrimitiveCollection<string> {"Software Engineering"},  "",  "Book",  new DateTime(2013, 4, 20), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenLectureAndEndDate01102021AndTypeVideoReturnsLecture9AndLecture10()
    {
        var options = new SearchOptions
        {
            SearchString = "Lecture",
            EndDate = new DateTime(2021, 10, 1),
            Type = "Video"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(9, "Lecture 9", new PrimitiveCollection<string> {"Software Engineering"}, "", "Video", new DateTime(2021, 9, 29), ""),
            new MaterialDto(10, "Lecture 10", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 1), "")
        }.OrderByDescending(m => m.Date);

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenDockerAndStartDate29102021AndEndDate29102021ReturnsLecture16()
    {
        var options = new SearchOptions
        {
            Tags = new PrimitiveSet<string> { "Docker" },
            StartDate = new DateTime(2021, 10, 29),
            EndDate = new DateTime(2021, 10, 29)
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenSoftwareEngineeringAndStartDate20042021AndTypeVideoReturnsLecture9()
    {
        var options = new SearchOptions
        {
            Tags = new PrimitiveSet<string> { "Software Engineering" },
            StartDate = new DateTime(2013, 4, 20),
            Type = "Video"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(9, "Lecture 9", new PrimitiveCollection<string> {"Software Engineering"}, "", "Video", new DateTime(2021, 9, 29), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenSoftwareEngineeringAndEndDate29092021AndTypeVideoReturnsLecture9()
    {
        var options = new SearchOptions
        {
            Tags = new PrimitiveSet<string> { "Software Engineering" },
            EndDate = new DateTime(2021, 9, 29),
            Type = "Video"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(9, "Lecture 9", new PrimitiveCollection<string> {"Software Engineering"}, "", "Video", new DateTime(2021, 9, 29), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenStartDate21022021AndEndDate29102021AndTypeBookReturnsCS90()
    {
        var options = new SearchOptions
        {
            StartDate = new DateTime(2021, 2, 26),
            EndDate = new DateTime(2021, 10, 29),
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(2, "C# 9.0 in a Nutshell", new PrimitiveCollection<string> {"C#"}, "", "Book", new DateTime(2021, 2, 26), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenLectureAndDockerAndStartDate29102021AndEndDate12112021ReturnsLecture16AndLecture20()
    {
        var options = new SearchOptions
        {
            SearchString = "Lecture",
            Tags = new PrimitiveSet<string> { "Docker" },
            StartDate = new DateTime(2021, 10, 29),
            EndDate = new DateTime(2021, 11, 12)
        };
        var result = await _repository.Search(options);
        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), ""),
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGiven9AndTagCSAndStartDate26022021AndTypeBookReturnsCS90()
    {
        var options = new SearchOptions
        {
            SearchString = "9",
            Tags = new PrimitiveSet<string>() { "C#" },
            StartDate = new DateTime(2021, 2, 26),
            Type = "Book"
        };
        var result = await _repository.Search(options);
        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(2, "C# 9.0 in a Nutshell", new PrimitiveCollection<string>() {"C#"}, "", "Book", new DateTime(2021, 2, 26), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenCTagMobileEndDate12112021TypeVideoReturnsLecture20()
    {
        var options = new SearchOptions
        {
            SearchString = "c",
            Tags = new PrimitiveSet<string>() { "Mobile" },
            EndDate = new DateTime(2021, 11, 12),
            Type = "Video"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(20, "Lecture 20", new PrimitiveCollection<string>() {"Mobile", "C#"}, "", "Video", new DateTime(2021, 11, 12), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGiven1StartDate29102021EndDate12112021TypeVideoReturnsLecture16AndLecture20()
    {
        var options = new SearchOptions
        {
            SearchString = "1",
            StartDate = new DateTime(2021, 10, 29),
            EndDate = new DateTime(2021, 11, 12),
            Type = "Video"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new PrimitiveCollection<string> {"Docker", "C#"}, "", "Video", new DateTime(2021, 10, 29), ""),
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenGivenCStartDate26022021EndDate29102021TypeBook()
    {
        var options = new SearchOptions
        {
            SearchString = "C",
            StartDate = new DateTime(2021, 2, 26),
            EndDate = new DateTime(2021, 10, 29),
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(2, "C# 9.0 in a Nutshell", new PrimitiveCollection<string>() {"C#"}, "", "Book", new DateTime(2021, 2, 26), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenTagCSStartDate26022021EndDate29102021TypeBook()
    {
        var options = new SearchOptions
        {
            Tags = new PrimitiveSet<string>() { "C#" },
            StartDate = new DateTime(2021, 2, 26),
            EndDate = new DateTime(2021, 10, 29),
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(2, "C# 9.0 in a Nutshell", new PrimitiveCollection<string>() {"C#"}, "", "Book", new DateTime(2021, 2, 26), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenCTagCSStartDate26022019EndDate12112021TypeBook()
    {
        var options = new SearchOptions
        {
            SearchString = "C",
            Tags = new PrimitiveSet<string>() { "C#" },
            StartDate = new DateTime(2021, 2, 26),
            EndDate = new DateTime(2021, 11, 12),
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new PrimitiveCollection<MaterialDto>()
        {
            new MaterialDto(2, "C# 9.0 in a Nutshell", new PrimitiveCollection<string>() {"C#"}, "", "Book", new DateTime(2021, 2, 26), "")
        };

        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task SearchGivenALimitShouldOnlyRetrunThatMany()
    {
        var options = new SearchOptions
        {
            SearchString = "",
            Tags = new PrimitiveSet<string>() { },
            Limit = 2
        };

        var result = await _repository.Search(options);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task SearchGivenAnOffsetShouldReturnDifferentResultsBasedOnOffset()
    {
        var optionsOffset0 = new SearchOptions
        {
            SearchString = "",
            Tags = new PrimitiveSet<string>() { },
            Limit = 2,
            Offset = 0
        };

        var optionsOffset2 = new SearchOptions
        {
            SearchString = "",
            Tags = new PrimitiveSet<string>() { },
            Limit = 2,
            Offset = 2
        };

        var resultOffset0 = await _repository.Search(optionsOffset0);
        var resultOffset2 = await _repository.Search(optionsOffset2);

        Assert.NotEqual(resultOffset0, resultOffset2);
    }

    [Fact]
    public async Task LimitBiggerThanResultShouldReturnAllResults()
    {
        var options = new SearchOptions
        {
            SearchString = "",
            Tags = new PrimitiveSet<string>() { },
            Limit = 100
        };

        var result = await _repository.Search(options);

        Assert.Equal(6, result.Count);
    }

    [Theory]
    [InlineData(Sortings.AZ)]
    [InlineData(Sortings.ZA)]
    [InlineData(Sortings.NEWEST)]
    [InlineData(Sortings.OLDEST)]
    public async Task OrderingBySamePropertyReturnsSameResult(Sortings sort)
    {
        var options = new SearchOptions
        {
            SearchString = "",
            Limit = 2,
            SortBy = sort
        };

        var result1 = await _repository.Search(options);
        var result2 = await _repository.Search(options);

        Assert.Equal(result1, result2);
    }

    [Theory]
    [InlineData(Sortings.NEWEST, Sortings.OLDEST)]
    [InlineData(Sortings.AZ, Sortings.ZA)]
    public async Task OrderingByDifferentPropertyReturnsDifferentResult(Sortings sort1, Sortings sort2)
    {
        var options1 = new SearchOptions
        {
            SearchString = "",
            Limit = 2,
            SortBy = sort1
        };
        var options2 = new SearchOptions
        {
            SearchString = "",
            Limit = 2,
            SortBy = sort2
        };

        var result1 = await _repository.Search(options1);
        var result2 = await _repository.Search(options2);

        Assert.NotEqual(result1, result2);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
