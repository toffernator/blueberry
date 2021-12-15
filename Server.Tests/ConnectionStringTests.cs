using blueberry.Server.Common;

namespace blueberry.Server.Tests.Common;

public class ConnectionStringTests
{
    [Fact]
    public void ThrowsGiven0Args()
    {
        Assert.Throws<ArgumentException>(() => ConnectionString.Read());
    }

    [Fact]
    public void ThrowsGiven1NullArg()
    {
        string? nullArg = null;
        Assert.Throws<ArgumentException>(() => ConnectionString.Read(nullArg));
    }

    [Fact]
    public void ReturnsArgGiven1Arg()
    {
        string expected = "Test ConnectionString";
        Assert.Equal(expected, ConnectionString.Read(expected));
    }

    [Fact]
    public void ReturnsFirstNonNullOrEmpty()
    {
        string expected = "Test ConnectionString";
        Assert.Equal(expected, ConnectionString.Read(null, "", "", null, expected));
    }
}
