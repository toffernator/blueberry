using Xunit;
using System;
using System.Threading.Tasks;
using blueberry.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using static blueberry.Core.Status;

namespace Infrastructure.Tests;

public class TagRepositoryTests : IDisposable
{
    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory");
        
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}