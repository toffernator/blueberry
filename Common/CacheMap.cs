using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace blueberry.Common;

public class CacheMap<K, V> : IDictionary<K, V>, IEnumerable, IEnumerable<KeyValuePair<K, V>>, IEnumerable<(K, V)>
    where K:  notnull
{
    private int capacity;
    private IDictionary<K, V> table = new Dictionary<K, V>();
    private Queue<K> queue;


    public CacheMap(int capacity)
    {
        this.capacity = capacity;
        queue = new Queue<K>(capacity);
    }

    public V this[K key] {
        get => table[key]; 
        set
        {
            MakeRoom();
            table[key] = value;
            queue.Enqueue(key);
        }
    }

    public ICollection<K> Keys => table.Keys;

    public ICollection<V> Values => table.Values;

    public int Count => table.Count;

    public bool IsReadOnly => table.IsReadOnly;

    public void Add(K key, V value)
    {
        MakeRoom();
        queue.Enqueue(key);
        table.Add(key, value);
    }

    public void Add(KeyValuePair<K, V> item)
    {
        MakeRoom();
        queue.Enqueue(item.Key);
        table.Add(item);
    }

    public void Clear()
    {
        table.Clear();
        queue.Clear();
    }

    public bool Contains(KeyValuePair<K, V> item) => table.Contains(item);

    public bool ContainsKey(K key) => table.ContainsKey(key);

    public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
    {
        table.CopyTo(array, arrayIndex);
        return;
    }

    public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => table.GetEnumerator();

    // Removing from queue: inspiration from https://kodify.net/csharp/queue/remove/
    public bool Remove(K key)
    {
       var result = table.Remove(key);
       queue = new Queue<K>(queue.Where(item => ! (key.Equals(item))));
       return result;

    }

    public bool Remove(KeyValuePair<K, V> item)
    {
        var result = table.Remove(item.Key);
       queue = new Queue<K>(queue.Where(key => ! (item.Key.Equals(key))));
       return result;
    }

    public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value) => table.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator()
    {
        return table.GetEnumerator();
    }

    IEnumerator<(K, V)> IEnumerable<(K, V)>.GetEnumerator()
    {
        return table.Select(kvpair => (kvpair.Key, kvpair.Value)).GetEnumerator();
    }

    /// <summary>
    /// Makes sure that the cache has capacity for insertion
    /// </summary>
    private void MakeRoom()
    {
        while (queue.Count >= capacity)
        {
            var removed = queue.Dequeue();
            table.Remove(removed);
        }
    }
}