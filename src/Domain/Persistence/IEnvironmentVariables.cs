using System.Collections.Generic;

namespace Obsidian.Persistence;

public interface IEnvironmentVariables : IDictionary<string, string>
{
    void Add(string key, string value);
    void Add(KeyValuePair<string, string> item);
    bool ContainsKey(string key);
    bool Remove(string key);
    bool Remove(KeyValuePair<string, string> item);
    bool TryGetValue(string key, out string value);
    string this[string key] { get; set; }
    ICollection<string> Keys { get; }
    ICollection<string> Values { get; }
    int Count { get; }
    bool IsReadOnly { get; }
    IEnumerator<KeyValuePair<string, string>> GetEnumerator();
    void Clear();
    bool Contains(KeyValuePair<string, string> item);
    void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex);

    void Update(string key, string value);
    void Update(KeyValuePair<string, string> item);
    void AddOrUpdate(string key, string value);
    void AddOrUpdate(KeyValuePair<string, string> item);
}