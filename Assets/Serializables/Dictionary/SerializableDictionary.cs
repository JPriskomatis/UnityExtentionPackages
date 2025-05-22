using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public SerializableKeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<SerializableKeyValuePair<TKey, TValue>> _items = new List<SerializableKeyValuePair<TKey, TValue>>();

    private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

    public Dictionary<TKey, TValue> Dictionary => _dictionary;

    public void OnBeforeSerialize()
    {
        _items.Clear();
        foreach (var kvp in _dictionary)
        {
            _items.Add(new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        _dictionary.Clear();
        foreach (var kvp in _items)
        {
            if (!_dictionary.ContainsKey(kvp.Key))
                _dictionary.Add(kvp.Key, kvp.Value);
        }
    }

    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }

    public void Add(TKey key, TValue value)
    {
        _dictionary[key] = value;
    }

    public void Remove(TKey key)
    {
        _dictionary.Remove(key);
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool ContainsValue(TValue value)
    {
        return _dictionary.ContainsValue(value);
    }

    public void Clear()
    {
        _dictionary.Clear();
        _items.Clear(); // Optional: keeps serialized list in sync
    }

    public int Count => _dictionary.Count;
}
