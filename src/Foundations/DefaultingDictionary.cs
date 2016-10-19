using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Foundations
{
    public class DefaultingDictionary<T, K> : IDictionary<T, K>
    {
        private readonly Dictionary<T,K> _this = 
            new Dictionary<T, K>();

        private readonly Func<T, K> _default;

        public DefaultingDictionary(Func<T, K> @default)
        {
            _default = @default;
        }

        public IEnumerator<KeyValuePair<T, K>> GetEnumerator()
        {
            return _this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<T, K> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _this.Clear();
        }

        public bool Contains(KeyValuePair<T, K> item)
        {
            return _this.Contains(item);
        }

        public void CopyTo(KeyValuePair<T, K>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<T, K> item)
        {
            return Remove(item.Key);
        }

        public int Count => _this.Count;
        public bool IsReadOnly => false;
        public void Add(T key, K value)
        {
            _this.Add(key, value);
        }

        public bool ContainsKey(T key)
        {
            return _this.ContainsKey(key);
        }

        public bool Remove(T key)
        {
            return _this.Remove(key);
        }

        public bool TryGetValue(T key, out K value)
        {
            return _this.TryGetValue(key, out value);
        }

        public K this[T key]
        {
            get {
                if (key == null)
                {
                    return _default(key);
                }

                return _this.ContainsKey(key) ? 
                    _this[key] : 
                    _default(key);
            }
            set
            {
                if (_this.ContainsKey(key))
                {
                    _this[key] = value;
                }
                else
                {
                    _this.Add(key, value);
                }
            }
        }

        public ICollection<T> Keys => _this.Keys;
        public ICollection<K> Values => _this.Values;
    }
}
