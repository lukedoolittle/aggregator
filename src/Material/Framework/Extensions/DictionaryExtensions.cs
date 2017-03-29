using System;
using System.Collections.Generic;
using System.Linq;
using Material.Framework.Collections;

namespace Material.Framework.Extensions
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
