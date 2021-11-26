using Xunit;
using System;
using System.Threading.Tasks;
using System.Linq;
using blueberry.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using blueberry.Infrastructure;
using static blueberry.Core.Status;

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

        context.Tags.AddRange(
            new Tag {Id = 1, Name = "Docker"},
            new Tag {Id = 2, Name = "Mobile"}
        );
        context.SaveChanges();

        context.Materials.AddRange(
            new Material {Id = 10, Title = "Lecture 10", ShortDescription = "", Tags = new [] {context.Tags.Find(1)}, ImageUrl = "", Type = "Video", Date = DateTime.Parse("10/01/2021")},
            new Material {Id = 16, Title = "Lecture 16", ShortDescription = "", Tags = new [] {context.Tags.Find(1)}, ImageUrl = "", Type = "Video", Date = DateTime.Parse("10/29/2021")},
            new Material {Id = 20, Title = "Lecture 20", ShortDescription = "", Tags = new [] {context.Tags.Find(2)}, ImageUrl = "", Type = "Video", Date = DateTime.Parse("11/12/2021")}
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
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker"}),
            new MaterialDto(20, "Lecture 20", new HashSet<string> {"Mobile"})
        };

        var isEqual = MaterialsEquals(expected, results);
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenLecture10ReturnsLecture10()
    {
        var options = new SearchOptions{SearchString = "Lecture 10"};
        var results = await _repository.Search(options);
        // Assuming that exactly one material matches "Lecture 10"
        var result = results.First();

        var isEqual = MaterialEquals(new MaterialDto(10, "Lecture 10", new HashSet<string>() {"Docker"}), result); 
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenLectureReturnsLecture10AndLecture16AndLecture20()
    {
        var options = new SearchOptions{SearchString = "Lecture"};
        var results = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker"}),
            new MaterialDto(20, "Lecture 20", new HashSet<string> {"Mobile"})
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
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker"})
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
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker"})
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
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker"}),
            new MaterialDto(20, "Lecture 20", new HashSet<string> {"Mobile"})
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
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker"}),
            new MaterialDto(20, "Lecture 20", new HashSet<string> {"Mobile"})
        };

        var isEqual = MaterialsEquals(expected, result);
        Assert.True(isEqual);
    }
    
    [Fact]
    public async Task SearchGivenEndDate29102021ReturnsLecture10AndLecture16()
    {
        var options = new SearchOptions{EndDate = DateTime.Parse("10/29/2021")};
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(10, "Lecture 10", new HashSet<string> {"Docker"}),
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker"}),
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
            new MaterialDto(16, "Lecture 16", new HashSet<string> {"Docker"}),
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

        // Magic Sauce to check that two enumerables have identical contents.
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
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~CharacterRepositoryTests()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
