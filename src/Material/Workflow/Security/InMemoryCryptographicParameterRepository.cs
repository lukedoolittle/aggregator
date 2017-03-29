using System;
using System.Collections.Generic;
using System.Linq;
using Material.Contracts;

namespace Material.Workflow.Security
{
    public class InMemoryCryptographicParameterRepository : ICryptographicParameterRepository
    {
        private static readonly Dictionary<Tuple<string, string>, Tuple<string, DateTimeOffset>> _dictionary = 
            new Dictionary<Tuple<string, string>, Tuple<string, DateTimeOffset>>();

        public bool TryInsertCryptographicParameterValue(
            string userId, 
            string parameterName, 
            string parameterValue, 
            DateTimeOffset timestamp)
        {
            var compositeKey = new Tuple<string, string>(userId, parameterName);

            if (_dictionary.ContainsKey(compositeKey))
            {
                return false;
            }

            _dictionary.Add(
                compositeKey, 
                new Tuple<string, DateTimeOffset>(parameterValue, timestamp));

            return true;
        }

        public Tuple<string, DateTimeOffset> GetCryptographicParameterValue(
            string userId, 
            string parameterName)
        {
            Tuple<string, DateTimeOffset> value = null;
            _dictionary.TryGetValue(new Tuple<string, string>(userId, parameterName), out value);
            return value;
        }

        public void DeleteCryptographicParameterValue(
            string userId, 
            string parameterName)
        {
            _dictionary.Remove(new Tuple<string, string>(userId, parameterName));
        }

        public void DeleteCryptographicParameterValues(string userId)
        {
            var itemsToRemove = _dictionary
                .Where(i => i.Key.Item1 == userId)
                .ToArray();

            foreach (var item in itemsToRemove)
            {
                _dictionary.Remove(item.Key);
            }
        }
    }
}
