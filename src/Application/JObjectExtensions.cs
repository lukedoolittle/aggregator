using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Aggregator.Framework.Exceptions;
using Material.Exceptions;

namespace Aggregator.Framework.Extensions
{
    //TODO: remove this after we do the FilterableRequest / TimestampRequest
	public static class JObjectExtensions
	{
        private const string DEFAULT_KEY = "value";

	    public static DateTimeOffset ExtractTimestamp(
            this JToken instance, 
            string timestampNavigation,
            string timestampFormat,
            string timestampOffsetNavigation,
            string timestampOffset)
	    {
	        if (instance == null)
	        {
	            throw new ArgumentNullException();
	        }

	        if (timestampNavigation == null)
	        {
	            return DateTimeOffset.MinValue;
	        }
	        else
	        {
	            var timestamp = instance.SelectToken(timestampNavigation).ToString();
	            string offset;

	            if (string.IsNullOrEmpty(timestampOffset))
	            {
	                offset = timestampOffsetNavigation == null ? 
                        null : 
                        instance.SelectToken(timestampOffsetNavigation).ToString();
	            }
	            else
	            {
	                offset = timestampOffset;
	            }

                return timestamp.ToDateTimeOffset(
                    timestampFormat, 
                    offset);
	        }
	    }

        public static IEnumerable<JObject> InContainer(this JToken instance)
	    {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (instance is JObject)
            {
                return new List<JObject> { (JObject)instance};
            }

            if (instance is JArray)
            {
                return instance.Select(item => item as JObject);
            }

            if (instance is JValue)
            {
                var result = new JObject
                {
                    [DEFAULT_KEY] = ((JValue)instance).Value.ToString()
                };
                return new List<JObject> { result };
            }

            throw new JsonResponseFormatException();
        }
	}
}
