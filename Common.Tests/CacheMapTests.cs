using Xunit;

namespace blueberry.Common.Tests;

public class CacheMapTests
{
    private CacheMap<int, int> cachemap = new CacheMap<int, int>(10);

    [Fact]
    public void AddsToMap()
    {
        cachemap.Add(0, 0);
        cachemap.Add(100, -34);
        AssertContains(0, 0);
        AssertContains(100, -34);

    }

    [Fact]
    public void CannotContainMoreThanCapacity()
    {
        cachemap.Add(1, 1);
        cachemap.Add(2, 2);
        cachemap.Add(3, 3);
        cachemap.Add(4, 4);
        cachemap.Add(5, 5);
        cachemap.Add(6, 6);
        cachemap.Add(7, 7);
        cachemap.Add(8, 8);
        cachemap.Add(9, 9);
        cachemap.Add(10, 10);
        cachemap.Add(11, 11);
        Assert.Equal(10, cachemap.Count);
    }

    [Fact]
    public void IsFIFO()
    {
        cachemap.Add(1, 1);
        cachemap.Add(2, 2);
        cachemap.Add(3, 3);
        cachemap.Add(4, 4);
        cachemap.Add(5, 5);
        cachemap.Add(6, 6);
        cachemap.Add(7, 7);
        cachemap.Add(8, 8);
        cachemap.Add(9, 9);
        cachemap.Add(10, 10);
        cachemap.Add(11, 11);
        AssertNotContains(1, 1);
        AssertContains(2, 2);
        AssertContains(3, 3);
        AssertContains(4, 4);
        AssertContains(5, 5);
        AssertContains(6, 6);
        AssertContains(7, 7);
        AssertContains(8, 8);
        AssertContains(9, 9);
        AssertContains(10, 10);
        AssertContains(11, 11);
    }

    [Fact]
    public void IsFIFO2()
    {
        cachemap.Add(1, 1);
        cachemap.Add(2, 2);
        cachemap.Add(3, 3);
        cachemap.Add(4, 4);
        cachemap.Add(5, 5);
        cachemap.Add(6, 6);
        cachemap.Add(7, 7);
        cachemap.Add(8, 8);
        cachemap.Add(9, 9);
        cachemap.Add(10, 10);
        cachemap.Add(11, 11);
        cachemap.Add(12, 12);
        AssertNotContains(1, 1);
        AssertNotContains(2, 2);
        AssertContains(3, 3);
        AssertContains(4, 4);
        AssertContains(5, 5);
        AssertContains(6, 6);
        AssertContains(7, 7);
        AssertContains(8, 8);
        AssertContains(9, 9);
        AssertContains(10, 10);
        AssertContains(11, 11);
        AssertContains(12, 12);
    }

    [Fact]
    public void AddsToMap_Indexer()
    {
        cachemap[0] = 0;
        cachemap[100] = -34;
        AssertContains(0, 0);
        AssertContains(100, -34);

    }

    [Fact]
    public void CannotContainMoreThanCapacity_Indexer()
    {
        cachemap[1] = 1 ;
        cachemap[2] = 2 ;
        cachemap[3] = 3 ;
        cachemap[4] = 4 ;
        cachemap[5] = 5 ;
        cachemap[6] = 6 ;
        cachemap[7] = 7 ;
        cachemap[8] = 8 ;
        cachemap[9] = 9 ;
        cachemap[10] = 10 ;
        cachemap[11] = 11 ;
        Assert.Equal(10, cachemap.Count);
    }

    [Fact]
    public void IsFIFO_Indexer()
    {
        cachemap[1] = 1 ;
        cachemap[2] = 2 ;
        cachemap[3] = 3 ;
        cachemap[4] = 4 ;
        cachemap[5] = 5 ;
        cachemap[6] = 6 ;
        cachemap[7] = 7 ;
        cachemap[8] = 8 ;
        cachemap[9] = 9 ;
        cachemap[10] = 10 ;
        cachemap[11] = 11 ;
        AssertNotContains(1, 1);
        AssertContains(2, 2);
        AssertContains(3, 3);
        AssertContains(4, 4);
        AssertContains(5, 5);
        AssertContains(6, 6);
        AssertContains(7, 7);
        AssertContains(8, 8);
        AssertContains(9, 9);
        AssertContains(10, 10);
        AssertContains(11, 11);
    }

    [Fact]
    public void IsFIFO2_Indexer()
    {
        cachemap[1] = 1 ;
        cachemap[2] = 2 ;
        cachemap[3] = 3 ;
        cachemap[4] = 4 ;
        cachemap[5] = 5 ;
        cachemap[6] = 6 ;
        cachemap[7] = 7 ;
        cachemap[8] = 8 ;
        cachemap[9] = 9 ;
        cachemap[10] = 10 ;
        cachemap[11] = 11 ;
        cachemap[12] = 12 ;
        AssertNotContains(1, 1);
        AssertNotContains(2, 2);
        AssertContains(3, 3);
        AssertContains(4, 4);
        AssertContains(5, 5);
        AssertContains(6, 6);
        AssertContains(7, 7);
        AssertContains(8, 8);
        AssertContains(9, 9);
        AssertContains(10, 10);
        AssertContains(11, 11);
        AssertContains(12, 12);
    }

    [Fact]
    public void Remove_removes()
    {
        cachemap.Add(1, 1);
        cachemap.Add(2, 2);
        cachemap.Add(3, 3);
        cachemap.Add(4, 4);
        cachemap.Remove(3);
        AssertContains(1, 1);
        AssertContains(2, 2);
        AssertNotContains(3, 3);
        AssertContains(4, 4);
    }

    [Fact]
    public void Clear_clears()
    {
        cachemap.Add(1, 1);
        cachemap.Add(2, 2);
        AssertContains(1, 1);
        AssertContains(2, 2);
        cachemap.Clear();
        AssertNotContains(1, 1);
        AssertNotContains(2, 2);
    }

    [Fact]
    public void TryGetValue_gives_value()
    {
        cachemap.Add(1, 1);
        var success = cachemap.TryGetValue(1, out int value);
        Assert.True(success);
        Assert.Equal(1, value);
    }

    [Fact]
    public void TryGetValue_returns_false()
    {
        cachemap.Add(1, 1);
        var success = cachemap.TryGetValue(2, out int value);
        Assert.False(success);
        Assert.Equal(default(int), value);
    }

    protected void AssertContains(int key, int value)
    {
        Assert.Contains((key, value), cachemap);
    }

    protected void AssertNotContains(int key, int value)
    {
        Assert.DoesNotContain((key, value), cachemap);
    }
}
