using System.Collections.Generic;

namespace blueberry.Common;

public class PrimitiveSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>, IReadOnlySet<T>
{
        private ICollection<T> _composite;

        public PrimitiveSet() => _composite = new HashSet<T>();

        public PrimitiveSet(ICollection<T> composite) => _composite = composite;

        public int Count { get => _composite.Count; }
        public bool IsReadOnly { get => _composite.IsReadOnly; }

        public void Add(T item) => _composite.Add(item);

        public void Clear() => _composite.Clear();

        public bool Contains(T item) => _composite.Contains(item);
        
        public void CopyTo(T[] array, int arrayIndex) => _composite.CopyTo(array, arrayIndex);

        public bool Remove(T item) => _composite.Remove(item);

		public IEnumerator<T> GetEnumerator() => _composite.GetEnumerator();

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

			var other = (ICollection<T>) obj;

			// Magic sauce to check that two enumerables have identical contents.
			// https://stackoverflow.com/questions/4576723/test-whether-two-ienumerablet-have-the-same-values-with-the-same-frequencies
			var itemGroups = _composite.ToLookup(t => t);
			var otherItemGroups = other.ToLookup(t => t);
			return itemGroups.Count() == otherItemGroups.Count()
				&& itemGroups.All(g => g.Count() == otherItemGroups[g.Key].Count());
        }
        
        public override int GetHashCode()
        {
			int hashCode = 0;
			foreach(T item in _composite)
			{
				hashCode += item.GetHashCode();
			}
            return hashCode;
        }
}