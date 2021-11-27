namespace Infrastructure.Tests;

public class MaterialRepositoryTests
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

        var Docker = new Tag{Name = "Docker"};
        var Mobile = new Tag{Name = "Mobile"};
        var SE = new Tag{Name = "Software Engineering"};
        var CS = new Tag{Name = "C#"};

        context.Materials.AddRange(
            new Material {Id = 1, Title = "OOSE", ShortDescription = "", Tags = new [] {SE}, ImageUrl = "", Type = "Book", Date = DateTime.Parse("04/20/2013")},
            new Material {Id = 2, Title = "C# 9.0 in a Nutshell", ShortDescription = "", Tags = new [] {CS}, ImageUrl = "", Type = "Book", Date = DateTime.Parse("02/26/2021")},
            new Material {Id = 9, Title = "Lecture 9", ShortDescription ="", Tags = new [] {SE}, ImageUrl = "", Type = "Video", Date = DateTime.Parse("09/29/2021")},
            new Material {Id = 10, Title = "Lecture 10", ShortDescription = "", Tags = new [] {Docker, CS}, ImageUrl = "", Type = "Video", Date = DateTime.Parse("10/01/2021")},
            new Material {Id = 16, Title = "Lecture 16", ShortDescription = "", Tags = new [] {Docker, CS}, ImageUrl = "", Type = "Video", Date = DateTime.Parse("10/29/2021")},
            new Material {Id = 20, Title = "Lecture 20", ShortDescription = "", Tags = new [] {Mobile, CS}, ImageUrl = "", Type = "Video", Date = DateTime.Parse("11/12/2021")}
        );
        context.SaveChanges();

        _context = context;
        _repository = new MaterialRepository(_context);
    }

    [Fact]
    public async Task SearchGivenEmptyStringReturnsEverything()
    {
        var options = new SearchOptions{SearchString = ""};
        var results = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new HashSet<string> {"Software Engineering"}),
            new MaterialDto(2, "C# 9.0 in a Nutshell", new HashSet<string> {"C#"}),
            new MaterialDto(9, "Lecture 9", new HashSet<string> {"Software Engineering"}),
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(20, "Lecture 20", new HashSet<string> {"Mobile", "C#"})
        };

        var isEqual = MaterialsEquals(expected, results);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenLecture10ReturnsLecture10()
    {
        var options = new SearchOptions{SearchString = "Lecture 10"};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"}),
        };

        var isEqual = MaterialsEquals(expected, result); 
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenLectureReturnsLecture9AndLecture10AndLecture16AndLecture20()
    {
        var options = new SearchOptions{SearchString = "Lecture"};
        var results = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(9, "Lecture 9", new HashSet<string> {"Software Engineering"}),
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(20, "Lecture 20", new HashSet<string> {"Mobile", "C#"})
        };

        var isEqual = MaterialsEquals(expected, results);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenTitleIgnoresCase()
    {
        var options = new SearchOptions{SearchString = "lEcTuRe 10"};
        var result = await _repository.Search(options);
IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);

    }

    [Fact]
    public async Task SearchGivenDockerTagReturnsLecture10AndLecture16()
    {
        var options = new SearchOptions{Tags = new HashSet<string>() {"Docker"}};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenDockerAndMobileTagReturnsLecture10AndLecture16AndLecture20()
    {
        var options = new SearchOptions{Tags = new HashSet<string>() {"Docker", "Mobile"}};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(20, "Lecture 20", new HashSet<string> {"Mobile", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenStartDate29102021ReturnsLecture16AndLecture20()
    {
        var options = new SearchOptions{StartDate = DateTime.Parse("10/29/2021")};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(20, "Lecture 20", new HashSet<string> {"Mobile", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }
    
    [Fact]
    public async Task SearchGivenEndDate29102021ReturnsOOSEAndCS90AndLecture9Lecture10AndLecture16()
    {
        var options = new SearchOptions{EndDate = DateTime.Parse("10/29/2021")};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {   
            new MaterialDto(1, "OOSE", new HashSet<string> {"Software Engineering"}),
            new MaterialDto(2, "C# 9.0 in a Nutshell", new HashSet<string> {"C#"}),
            new MaterialDto(9, "Lecture 9", new HashSet<string> {"Software Engineering"}),
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenTypeBookReturnsOOSEAndCS90()
    {
        var options = new SearchOptions{Type = "Book"};
        var result = await _repository.Search(options);
        
        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new HashSet<string> {"Software Engineering"}),
            new MaterialDto(2, "C# 9.0 in a Nutshell", new HashSet<string> {"C#"}),
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenStartDate29102021AndEndDate29102021ReturnsLecture16()
    {
        var options = new SearchOptions{StartDate =  DateTime.Parse("10/29/2021"), EndDate = DateTime.Parse("10/29/2021")};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"}),
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGiven10AndDockerTagReturnsLecture10()
    {
        var options = new SearchOptions{SearchString = "10", Tags = new HashSet<string>() {"Docker"}};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGiven10AndStartDate29102021ReturnsNothing()
    {
        var options = new SearchOptions{SearchString = "10", StartDate = DateTime.Parse("10/29/2021")};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>();

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGiven10AndEndDate29102021ReturnsLecture10()
    {
        var options = new SearchOptions{SearchString = "10", EndDate = DateTime.Parse("10/29/2021")};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenLectureAndDockerAndStartDate29102021ReturnsLecture16()
    {
        var options = new SearchOptions{SearchString = "Lecture", Tags = new HashSet<string>() {"Docker"}, StartDate = DateTime.Parse("10/29/2021")};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }


    [Fact]
    public async Task SearchGivenDockerTagAndStartDate29102021ReturnsLecture16()
    {
        var options = new SearchOptions{Tags = new HashSet<string> {"Docker"}, StartDate = DateTime.Parse("10/29/2021")};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }
    
    [Fact]
    public async Task SearchGivenDockerTagAndEndDate29102021ReturnsLecture10AndLecture16()
    {
        var options = new SearchOptions{Tags = new HashSet<string> {"Docker"}, EndDate = DateTime.Parse("10/29/2021")};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenTypeBookAndLectureReturnsNothing()
    {
        var options = new SearchOptions{SearchString = "Lecture", Type = "Book"};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>();

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenTypeBookAndSoftwareEngineeringRetunsOOSE()
    {
        var options = new SearchOptions{Tags = new HashSet<string> {"Software Engineering"}, Type = "Book"};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new HashSet<string> {"Software Engineering"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenTypeVideoAndStartDate12112021ReturnsLecture20()
    {
        var options = new SearchOptions{StartDate = DateTime.Parse("11/12/2021"), Type = "Video"};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(20, "Lecture 20", new HashSet<string> {"Mobile", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenTypeVideoAndEndDate29102021ReturnsLecture10AndLecture16()
    {
        var options = new SearchOptions{EndDate = DateTime.Parse("10/29/2021"), Type = "Video"};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(9, "Lecture 9", new HashSet<string> {"Software Engineering"}),
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGiven10AndDockerAndStartDate01102021ReturnsLecture10()
    {

        var options = new SearchOptions
        {
            SearchString = "10", 
            Tags = new HashSet<string>() {"Docker"},
            StartDate = DateTime.Parse("10/01/2021")
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"}),
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGiven16AndDockerAndEndDate29102021ReturnsLecture16()
    {
        var options = new SearchOptions
        {
            SearchString = "16", 
            Tags = new HashSet<string>() {"Docker"},
            EndDate = DateTime.Parse("10/29/2021")
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"}),
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenOAndSoftwareEngineeringAndTypeBookReturnsOOSE()
    {
        var options = new SearchOptions
        {
            SearchString = "O", 
            Tags = new HashSet<string>() {"Software Engineering"},
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new HashSet<string> {"Software Engineering"}),
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenLectureAndStartDate01102021AndEndDate29102021ReturnsLecture10AndLecture16()
    {
        var options = new SearchOptions
        {
            SearchString = "Lecture",
            StartDate = DateTime.Parse("10/01/2021"),
            EndDate = DateTime.Parse("10/29/2021")
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"}),
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenOAndStartDate20042013AndTypeBookReturnsOOSE()
    {
        var options = new SearchOptions
        {
            SearchString = "O",
            StartDate = DateTime.Parse("04/20/2013"),
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(1, "OOSE", new HashSet<string> {"Software Engineering"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenLectureAndEndDate01102021AndTypeVideoReturnsLecture9AndLecture10()
    {
        var options = new SearchOptions
        {
            SearchString = "Lecture",
            EndDate = DateTime.Parse("10/01/2021"),
            Type = "Video"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(9, "Lecture 9", new HashSet<string> {"Software Engineering"}),
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenDockerAndStartDate29102021AndEndDate29102021ReturnsLecture16()
    {
        var options = new SearchOptions
        {
            Tags = new HashSet<string> {"Docker"},
            StartDate = DateTime.Parse("10/29/2021"),
            EndDate = DateTime.Parse("10/29/2021")
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenSoftwareEngineeringAndStartDate20042021AndTypeVideoReturnsLecture9()
    {
        var options = new SearchOptions
        {
            Tags = new HashSet<string> {"Software Engineering"},
            StartDate = DateTime.Parse("04/20/2013"),
            Type = "Video"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(9, "Lecture 9", new HashSet<string> {"Software Engineering"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenSoftwareEngineeringAndEndDate29092021AndTypeVideoReturnsLecture9()
    {
        var options = new SearchOptions
        {
            Tags = new HashSet<string> {"Software Engineering"},
            EndDate = DateTime.Parse("09/29/2021"),
            Type = "Video"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(9, "Lecture 9", new HashSet<string> {"Software Engineering"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenStartDate21022021AndEndDate29102021AndTypeBookReturnsCS90()
    {
        var options = new SearchOptions
        {
            StartDate = DateTime.Parse("02/26/2021"),
            EndDate = DateTime.Parse("10/29/2021"),
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(2, "C# 9.0 in a Nutshell", new HashSet<string> {"C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenLectureAndDockerAndStartDate29102021AndEndDate12112021ReturnsLecture16AndLecture20()
    {
        var options = new SearchOptions
        {
            SearchString = "Lecture",
            Tags = new HashSet<string> {"Docker"},
            StartDate = DateTime.Parse("10/29/2021"),
            EndDate = DateTime.Parse("11/12/2021")
        };
        var result = await _repository.Search(options);
        
        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"}),
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGiven9AndTagCSAndStartDate26022021AndTypeBookReturnsCS90()
    {
        var options = new SearchOptions
        {
            SearchString = "9",
            Tags = new HashSet<string>() {"C#"},
            StartDate = DateTime.Parse("02/26/2021"),
            Type = "Book"
        };
        var result = await _repository.Search(options);
        
        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(2, "C# 9.0 in a Nutshell", new HashSet<string>() {"C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenCTagMobileEndDate12112021TypeVideoReturnsLecture20()
    {
        var options = new SearchOptions
        {
            SearchString = "c",
            Tags = new HashSet<string>() {"Mobile"},
            EndDate = DateTime.Parse("11/12/2021"),
            Type = "Video"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(20, "Lecture 20", new HashSet<string>() {"Mobile", "C#"})
        };
        
        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGiven1StartDate29102021EndDate12112021TypeVideoReturnsLecture16AndLecture20()
    {
        var options = new SearchOptions
        {
            SearchString = "1",
            StartDate = DateTime.Parse("10/29/2021"),
            EndDate = DateTime.Parse("11/12/2021"),
            Type = "Video"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker", "C#"}),
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenGivenCStartDate26022021EndDate29102021TypeBook()
    {
        var options = new SearchOptions
        {
            SearchString = "C",
            StartDate = DateTime.Parse("02/26/2021"),
            EndDate = DateTime.Parse("10/29/2021"),
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(2, "C# 9.0 in a Nutshell", new HashSet<string>() {"C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenTagCSStartDate26022021EndDate29102021TypeBook()
    {
        var options = new SearchOptions
        {
            Tags = new HashSet<string>() {"C#"},
            StartDate = DateTime.Parse("02/26/2021"),
            EndDate = DateTime.Parse("10/29/2021"),
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(2, "C# 9.0 in a Nutshell", new HashSet<string>() {"C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenCTagCSStartDate26022019EndDate12112021TypeBook()
    {
        var options = new SearchOptions
        {
            SearchString = "C",
            Tags = new HashSet<string>() {"C#"},
            StartDate = DateTime.Parse("02/26/2021"),
            EndDate = DateTime.Parse("11/12/2021"),
            Type = "Book"
        };
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(2, "C# 9.0 in a Nutshell", new HashSet<string>() {"C#"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }

    private bool MaterialsEquals(IEnumerable<MaterialDto> materials, IEnumerable<MaterialDto> others)
    {
        if (materials.Count() != others.Count())
        {
            return false;
        }

        var mList = materials.OrderBy(m => m.Id).ToList();
        var oList = others.OrderBy(m => m.Id).ToList();
        others.GetEnumerator().MoveNext();
        for (int i = 0; i < mList.Count(); i++)
        {
            if (!MaterialEquals(mList[i], oList[i]))
            {
                return false;
            }
        }

        return true;
    }

    private bool MaterialEquals(MaterialDto material, MaterialDto other)
    {
        if (material.Id != other.Id && material.Title != other.Title)
        {
            return false;
        }

        // Magic sauce to check that two enumerables have identical contents.
        // https://stackoverflow.com/questions/4576723/test-whether-two-ienumerablet-have-the-same-values-with-the-same-frequencies
        var tagsGroups = material.Tags.ToLookup(t => t);
        var otherTagsGroups = other.Tags.ToLookup(t => t);
        return tagsGroups.Count() == otherTagsGroups.Count()
            && tagsGroups.All(g => g.Count() == otherTagsGroups[g.Key].Count());
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
