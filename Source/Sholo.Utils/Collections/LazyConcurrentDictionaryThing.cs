using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Sholo.Utils.Collections;

[PublicAPI]
public class LazyConcurrentDictionaryThing<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{
    private ConcurrentDictionary<TKey, Lazy<TValue>> Dictionary { get; } = new();

    public TValue GetOrAdd(TKey key, Func<TValue> valueFactory) => Dictionary.GetOrAdd(key, _ => new Lazy<TValue>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication)).Value;

    public TValue this[TKey key] => Dictionary[key].Value;
    public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Dictionary.Select(x => new KeyValuePair<TKey, TValue>(x.Key, x.Value.Value)).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Remove(TKey key, out TValue value)
    {
        var result = Dictionary.Remove(key, out var val);

        if (result)
        {
            value = val.Value;
            return true;
        }
        else
        {
            value = default!;
            return false;
        }
    }

    public bool IsFixedSize => false;
    public bool IsReadOnly => false;

    public void Clear()
    {
        Dictionary.Clear();
    }

    public int Count => Dictionary.Count;

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (Dictionary.TryGetValue(key, out var lazyValue))
        {
            value = lazyValue.Value;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    public ICollection<TKey> Keys => Dictionary.Keys;
    public ICollection<TValue> Values => Dictionary.Values.Select(x => x.Value).ToArray();
}
