using System.Collections;
using System.Collections.Generic;

namespace blueberry.Common;

public class PrimitiveSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>, IReadOnlySet<T>, IEquatable<ISet<T>>
{
    public ISet<T> _backing;
    public int Count => _backing.Count;
    public bool IsReadOnly => _backing.IsReadOnly;

    public PrimitiveSet(IEnumerable<T> backing) => _backing = backing.ToHashSet();
    public PrimitiveSet(ISet<T> backing) => _backing = backing;
    public PrimitiveSet() => _backing = new HashSet<T>();

    public void Add(T item) => _backing.Add(item);

    public void Clear() => _backing.Clear();

    public bool Contains(T item) => _backing.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _backing.CopyTo(array, arrayIndex);

    public void ExceptWith(IEnumerable<T> other) => _backing.ExceptWith(other);

    public IEnumerator<T> GetEnumerator() => _backing.GetEnumerator();

    public void IntersectWith(IEnumerable<T> other) => _backing.IntersectWith(other);

    public bool IsProperSubsetOf(IEnumerable<T> other) => _backing.IsProperSubsetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other) => _backing.IsProperSupersetOf(other);

    public bool IsSubsetOf(IEnumerable<T> other) => _backing.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other) => _backing.IsSubsetOf(other);

    public bool Overlaps(IEnumerable<T> other) => _backing.Overlaps(other);

    public bool Remove(T item) => _backing.Remove(item);

    public bool SetEquals(IEnumerable<T> other) => _backing.SetEquals(other);

    public void SymmetricExceptWith(IEnumerable<T> other) => _backing.SymmetricExceptWith(other);

    public void UnionWith(IEnumerable<T> other) => _backing.UnionWith(other);
    bool ISet<T>.Add(T item) => _backing.Add(item);

    IEnumerator IEnumerable.GetEnumerator() => _backing.GetEnumerator();

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

		return Equals(obj as ISet<T>);
    }

    public bool Equals(ISet<T>? other)
    {
        if (other == null)
        {
            return false;
        }

		// Magic sauce to check that two enumerables have identical contents.
		// https://stackoverflow.com/questions/4576723/test-whether-two-ienumerablet-have-the-same-values-with-the-same-frequencies
		var itemGroups = _backing.ToLookup(t => t);
		var otherItemGroups = other.ToLookup(t => t);
		return itemGroups.Count() == otherItemGroups.Count()
			&& itemGroups.All(g => g.Count() == otherItemGroups[g.Key].Count());
    }
    
    public override int GetHashCode()
    {
		int hashCode = 0;
		foreach(T item in _backing)
		{
            if (item != null)
            {
			    hashCode += item.GetHashCode();
            }
		}
        return hashCode;
    }
}