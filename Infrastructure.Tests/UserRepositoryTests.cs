using Xunit;
using System;
using System.Threading.Tasks;
using blueberry.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using static blueberry.Core.Status;


namespace blueberry.Infrastructure.Tests;

public class UserRepositoryTests : IDisposable
{
    private readonly BlueberryContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory");
        connection.Open();
        var builder = new DbContextOptionsBuilder<BLueberryContext>();
        builder.UseSqlite(connection);
        var context = new BlueberryContext(builder.Options);
        context.Database.EnsureCreated();

        var react = new TagCreateDto("React");
        var blazor = new TagCreateDto("Blazor");

        context.Users.AddRange(
            new User{ Id = 1, Name = "Mie", Interests = new[] { blazor, react } }
        );
        

        context.SaveChanges();
        _context = context;
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task Test1()
    {
        var user = new UserCreateDto
        {
            Id = 2,
            Name = "Fie",
            Interests =  new HashSet<string> { "Blazor", "React" }
        };

        var created = await _repository.Create(user);

        Assert.Equal(1, created.Id);
        Assert.Equal("Mie", created.Name);
        Assert.True(created.Interests.SetEquals(new[] { "Blazor", "React" }));
    }
}