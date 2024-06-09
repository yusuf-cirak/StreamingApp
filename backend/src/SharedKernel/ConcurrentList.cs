using System.Collections;
using System.Collections.Concurrent;

namespace SharedKernel;

public sealed class ConcurrentList<T> : ICollection<T>
{
    private readonly ConcurrentDictionary<T, object> _store;

    public ConcurrentList(IEnumerable<T> items = null)
    {
        var prime = (items ?? Enumerable.Empty<T>()).Select(x => new KeyValuePair<T, object>(x, null));
        _store = new ConcurrentDictionary<T, object>(prime);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _store.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        if (_store.TryAdd(item, null) == false)
            throw new ApplicationException("Unable to concurrently add item to list");
    }

    public void Clear()
    {
        _store.Clear();
    }

    public bool Contains(T item)
    {
        return item != null && _store.ContainsKey(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _store.Keys.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return _store.Keys.Remove(item);
    }

    public int Count => _store.Count;

    public bool IsReadOnly => _store.Keys.IsReadOnly;
}