using System;
using System.Linq;
using Material.Framework.Collections;
using Material.Framework.Extensions;
using Material.HttpClient.Content;

namespace Material.HttpClient.ParameterHandlers
{
    public class QuerystringParameterHandler : IParameterHandler
    {
        public void AddParameters(
            RequestParameters message, 
            HttpValueCollection parameters)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            if (parameters == null || !parameters.Any())
            {
                return;
            }

            message.Address = message
                                    .Address
                                    .AddEncodedQuerystring(
                                        parameters);
        }
    }
}
