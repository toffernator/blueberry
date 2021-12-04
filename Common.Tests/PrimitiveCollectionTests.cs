using blueberry.Common;
using System.Collections.Generic;
using Xunit;

namespace blueberry.Common.Tests;

public class PrimitiveCollectionTests
{
    [Theory]
    [InlineData(new [] {1, 2, 3}, new [] {1, 2, 3})]
    [InlineData(new [] {1, 2, 3}, new [] {3, 2, 1})]
    public void EqualsGivenSameContentsReturnsTrue(int[] aThing, int[] anotherThing)
    {

        Assert.True(new PrimitiveCollection<int>(aThing).Equals(new PrimitiveCollection<int>(anotherThing)));
    }

    [Theory]
    [InlineData(new [] {1, 2, 3}, new [] {1, 2, 3})]
    [InlineData(new [] {1, 2, 3}, new [] {3, 2, 1})]
    public void EqualsGivenSameContentsButDifferentCollectionTypeReturnsTrue(int[] aThing, int[] anotherThing)
    {

        Assert.True(new PrimitiveCollection<int>(aThing).Equals(new List<int>(anotherThing)));
    }

    [Fact]
    public void EqualsGivenNullReturnsFalse()
    {
        Assert.False(new PrimitiveCollection<int>().Equals(null));
    }

    [Theory]
    [InlineData(new [] {1, 2, 3}, new [] {1, 2, 4})]
    [InlineData(new [] {1, 2, 3}, new [] {4, 2, 0})]
    public void EqualsGivenDifferentContentsTypeReturnsFalse(int[] aThing, int[] anotherThing)
    {

        Assert.False(new PrimitiveCollection<int>(aThing).Equals(new PrimitiveCollection<int>(anotherThing)));
    }

    [Theory]
    [InlineData(new [] {1, 2, 3}, new [] {1, 2, 4})]
    [InlineData(new [] {1, 2, 3}, new [] {4, 2, 0})]
    public void EqualsGivenDifferentContentsAndDifferentCollectionTypeReturnsFalse(int[] aThing, int[] anotherThing)
    {

        Assert.False(new PrimitiveCollection<int>(aThing).Equals(new List<int>(anotherThing)));
    }
}
