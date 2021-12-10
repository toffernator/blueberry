using blueberry.Server.Common;

namespace blueberry.Server.Tests.Common;

public class ConnectionStringTests
{
    [Fact]
    public async Task ThrowsGiven0Args()
    {
        Assert.Throws<ArgumentException>(() => ConnectionString.Read());
    }

    [Fact]
    public async Task ThrowsGiven1NullArg()
    {
        string? nullArg = null;
        Assert.Throws<ArgumentException>(() => ConnectionString.Read(nullArg));
    }

    [Fact]
    public async Task ReturnsArgGiven1Arg()
    {
        string expected = "Test ConnectionString";
        Assert.Equal(expected, ConnectionString.Read(expected));
    }

    [Fact]
    public async Task ReturnsFirstNonNullOrEmpty()
    {
        string expected = "Test ConnectionString";
        Assert.Equal(expected, ConnectionString.Read(null, "", "", null, expected));
    }
}
