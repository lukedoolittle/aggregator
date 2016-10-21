using System;
using System.Collections.Generic;
using System.Linq;
using Foundations.Collections;

namespace Foundations.Extensions
{
    public static class DictionaryExtensions
    {
        public static HttpValueCollection ToHttpValueCollection(
            this IDictionary<string, string> instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return instance
                .Select(d => new HttpValue(d.Key, d.Value))
                .ToList();
        }
    }
}
