using System.Collections;
using System.Collections.Generic;

namespace blueberry.Common;

public class PrimitiveCollection<T> : ICollection<T>, IReadOnlyCollection<T>, IEquatable<ICollection<T>>
{
    private ICollection<T> _backing;


    public PrimitiveCollection(IEnumerable<T> backing) => _backing = backing.ToList();
    public PrimitiveCollection(ICollection<T> backing) => _backing = backing;
    public PrimitiveCollection() => _backing = new List<T>();

    public virtual int Count { get => _backing.Count; }

    public virtual bool IsReadOnly { get => _backing.IsReadOnly; }

    public virtual void Add(T item) => _backing.Add(item);

    public virtual void Clear() => _backing.Clear();

    public virtual bool Contains(T item) => _backing.Contains(item);

    public virtual void CopyTo(T[] array, int arrayIndex) => _backing.CopyTo(array, arrayIndex);

    public virtual bool Remove(T item) => _backing.Remove(item);

    public virtual IEnumerator<T> GetEnumerator() => _backing.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>  _backing.GetEnumerator();

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

		return Equals(obj as ICollection<T>);
    }

    public bool Equals(ICollection<T>? other)
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