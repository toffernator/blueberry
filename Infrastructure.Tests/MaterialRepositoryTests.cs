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
            new Tag {Id = 1, Name = "Docker"}
        );
        context.SaveChanges();

        var docker = new HashSet<Tag>(new [] {context.Tags.Find(1)});
        context.Materials.AddRange(
            new Material {Id = 8, Title = "Lecture 8", ShortDescription = "", Tags = docker, ImageUrl = "", Type = "Video", Date = DateTime.Today},
            new Material {Id = 14, Title = "Lecture 14", ShortDescription = "", Tags = docker, ImageUrl = "", Type = "Video", Date = DateTime.Today}
        );
        context.SaveChanges();

        _context = context;
        _repository = new MaterialRepository(_context);
    }

    [Fact]
    public async Task SearchGivenLecture8ReturnsLecture8()
    {
        var options = new SearchOptions("Lecture 8", null, null, null);
        var results = await _repository.Search(options);
        // Assuming that exactly one material matches "Lecture 8"
        var result = results.First();

        var isEqual = MaterialEquals(new MaterialDto(8, "Lecture 8", new HashSet<string>() {"Docker"}), result); 
        Assert.True(isEqual);
    }

    [Fact]
    public async Task SearchGivenDockerTagReturnsLecture8AndLecture14()
    {
        var options = new SearchOptions("", new HashSet<string>() {"Docker"}, null, null);
        var result = await _repository.Search(options);

        IEnumerable<MaterialDto> expected = new HashSet<MaterialDto>()
        {
            new MaterialDto(8, "Lecture 8", new HashSet<string> {"Docker"}),
            new MaterialDto(14, "Lecture 14", new HashSet<string> {"Docker"})
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
