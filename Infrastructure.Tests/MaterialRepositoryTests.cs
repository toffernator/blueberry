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

        // TODO: Populate with data

        _context = context;
        _repository = new MaterialRepository(_context);
    }

    [Fact]
    public async Task Search_given_docker_tag_returns_lectures_8_and_14()
    {
        var criteria =  _context.Materials.AsQueryable()
            .Where(m => m.Tags.Contains(new Tag("Docker")))
            .Select(m => new MaterialDto(m.Id, m.Title, NamesOf(m.Tags)));

        var result = await _repository.Search(criteria);

        Assert.Collection(result,
            r => Assert.Equal(new MaterialDto(1, "Lecture 8", new List<string>() { "Docker" }), r),
            r => Assert.Equal(new MaterialDto(2, "Lecture 14", new List<string>() { "Docker" }), r)
        );
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

    private IEnumerable<string> NamesOf(ICollection<Tag> tags) 
    {
        ISet<string> tagNames = new HashSet<string>(); 
        foreach(Tag t in tags)
        {
            tagNames.Add(t.Name);
        }
        return tagNames;
    }
}
