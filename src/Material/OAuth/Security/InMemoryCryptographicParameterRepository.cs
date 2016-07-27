using System;
using System.Collections.Generic;
using Material.Contracts;

namespace Material.OAuth
{
    public class InMemoryCryptographicParameterRepository : ICryptographicParameterRepository
    {
        private static readonly Dictionary<Tuple<string, string>, Tuple<string, DateTimeOffset>> _dictionary = 
            new Dictionary<Tuple<string, string>, Tuple<string, DateTimeOffset>>();

        public void SetCryptographicParameterValue(
            string userId, 
            string parameterName, 
            string parameterValue, 
            DateTimeOffset timestamp)
        {
            _dictionary.Add(
                new Tuple<string, string>(userId, parameterName), 
                new Tuple<string, DateTimeOffset>(parameterValue, timestamp));
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
    }
}
