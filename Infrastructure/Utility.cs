namespace Infrastructure;

public class Utility
{
    public static bool MaterialsEquals(IEnumerable<MaterialDto> materials, IEnumerable<MaterialDto> others)
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

    public static bool MaterialEquals(MaterialDto material, MaterialDto other)
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
}