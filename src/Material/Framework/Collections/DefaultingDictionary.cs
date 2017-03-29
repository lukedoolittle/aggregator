using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Material.Framework.Collections
{
    public class DefaultingDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey,TValue> _this = 
            new Dictionary<TKey, TValue>();

        private readonly Func<TKey, TValue> _default;

        public DefaultingDictionary(Func<TKey, TValue> @default)
        {
            _default = @default;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _this.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _this.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public int Count => _this.Count;
        public bool IsReadOnly => false;
        public void Add(TKey key, TValue value)
        {
            _this.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return _this.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return _this.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _this.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
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

        public ICollection<TKey> Keys => _this.Keys;
        public ICollection<TValue> Values => _this.Values;
    }
}
