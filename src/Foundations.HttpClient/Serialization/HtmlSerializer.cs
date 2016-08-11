using System;
using System.Linq;
using Foundations.Serialization;

namespace Foundations.HttpClient.Serialization
{
    public class HtmlSerializer : ISerializer
    {
        public string Serialize<T>(T item)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(string item)
        {
            var jsonArray = HttpUtility
                .ParseQueryString(item)
                .Select(q => $"\"{q.Key}\" : \"{q.Value}\"");

            var json = "{ " + string.Join(",", jsonArray) + " }";

            var newtonsoft = json.AsEntity<T>();

            var dotnet = new JsonSerializer().Deserialize<T>(json);

            return dotnet;
        }
    }
}
