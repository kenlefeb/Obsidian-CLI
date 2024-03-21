using System.Collections;
using System.Collections.Generic;

namespace Obsidian.Persistence
{
    public class EnvironmentVariables : IEnvironmentVariables
    {
        private readonly IDictionary<string, string> _environment = new Dictionary<string, string>();

        public EnvironmentVariables() : this(System.Environment.GetEnvironmentVariables()) { }

        public EnvironmentVariables(IDictionary environment)
        {
            foreach (DictionaryEntry entry in environment)
            {
                if (entry.Key is string key && entry.Value is string value)
                {
                    _environment.Add(key, value);
                }
            }
        }

        public void Add(string key, string value)
        {
            _environment.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _environment.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _environment.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return _environment.TryGetValue(key, out value);
        }

        public string this[string key]
        {
            get => _environment[key];
            set => _environment[key] = value;
        }

        public ICollection<string> Keys => _environment.Keys;

        public ICollection<string> Values => _environment.Values;

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _environment.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_environment).GetEnumerator();
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _environment.Add(item);
        }

        public void Clear()
        {
            _environment.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _environment.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            _environment.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return _environment.Remove(item);
        }

        public int Count => _environment.Count;

        public bool IsReadOnly => _environment.IsReadOnly;
    }
}
