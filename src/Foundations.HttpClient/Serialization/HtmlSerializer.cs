using System;
using System.Globalization;
using System.Linq;
using Foundations.Collections;

namespace Foundations.HttpClient.Serialization
{
    public class HtmlSerializer : ISerializer
    {
        public string Serialize(object entity)
        {
            throw new NotImplementedException();
        }

        public string Serialize<TEntity>(TEntity entity)
        {
            return Serialize((object)entity);
        }

        public TEntity Deserialize<TEntity>(
            string entity, 
            string dateTimeFormat)
        {
            var jsonArray = HttpUtility
                .ParseQueryString(entity)
                .Select(q => string.Format(
                    CultureInfo.CurrentCulture, 
                    "\"{0}\" : \"{1}\"", 
                    q.Key, 
                    q.Value));

            var json = "{ " + string.Join(",", jsonArray) + " }";

            var dotnet = new JsonSerializer().Deserialize<TEntity>(
                json, 
                dateTimeFormat);

            return dotnet;
        }

        public TEntity Deserialize<TEntity>(string entity)
        {
            return Deserialize<TEntity>(entity, null);
        }
    }
}
