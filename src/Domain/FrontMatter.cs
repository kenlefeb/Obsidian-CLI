using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Obsidian.Domain;

public class FrontMatter : IDictionary<string, object>
{
    private Dictionary<string, object> _yaml;

    public static FrontMatter Parse(string yaml)
    {
        var normalized = Normalize(yaml);
        var deserializer = new YamlDotNet.Serialization.Deserializer();
        var parsed = deserializer.Deserialize<Dictionary<string, object>>(normalized);
        return new FrontMatter
        {
            _yaml = parsed
        };
    }

    private static string Normalize(string yaml)
    {
        var pattern = @"^---\r?\n(.*?)\r?\n---$";
        var options = RegexOptions.Singleline | RegexOptions.Multiline;
        var match = Regex.Match(yaml, pattern, options);
        if (match.Success)
        {
            return match.Groups[1].Value;
        }
        else
        {
            return yaml;
        }
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return _yaml.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_yaml).GetEnumerator();
    }

    public void Add(KeyValuePair<string, object> item)
    {
        _yaml.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _yaml.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item)
    {
        return _yaml.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<string, object> item)
    {
        return _yaml.Remove(item.Key);
    }

    public int Count => _yaml.Count;

    public bool IsReadOnly => ((IDictionary<string, object>)_yaml).IsReadOnly;

    public void Add(string key, object value)
    {
        _yaml.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
        return _yaml.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return _yaml.Remove(key);
    }

    public bool TryGetValue(string key, out object value)
    {
        return _yaml.TryGetValue(key, out value);
    }

    public object this[string key]
    {
        get => _yaml[key];
        set => _yaml[key] = value;
    }

    public ICollection<string> Keys => ((IDictionary<string, object>)_yaml).Keys;

    public ICollection<object> Values => ((IDictionary<string, object>)_yaml).Values;
}