using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using BatmansBelt.Serialization;

namespace Aggregator.Framework.Serialization
{
    public static partial class SerializationExtensions
    {
        public static TEntity AsEntity<TEntity>(
            this NameValueCollection source,
            bool withType = true)
        {
            var dictionary = source.Cast<string>()
                .ToDictionary(p => p, p => (object)source[p]);

            if (withType)
            {
                Type rootType = typeof(TEntity);
                var typedDictionary = new Dictionary<string, object>
                {
                    {"$type", $"{rootType.FullName}, {rootType.Assembly.GetName().Name}"}
                };
                foreach (var item in dictionary)
                {
                    typedDictionary.Add(item.Key, item.Value);
                }
                dictionary = typedDictionary;
            }

            return dictionary.AsEntity<TEntity>(true);
        }
    }
}
