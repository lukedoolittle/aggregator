using System;
using System.Linq;
using Foundations.Collections;
using Foundations.Extensions;
using Foundations.HttpClient.Request;

namespace Foundations.HttpClient.ParameterHandlers
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
