using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Foundations.Collections;

namespace Foundations.HttpClient.ParameterHandlers
{
    public class BodyParameterHandler : IParameterHandler
    {
        public void AddParameters(
            HttpRequestMessage message,
            HttpValueCollection parameters)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (parameters == null || !parameters.Any())
            {
                return;
            }

            message.Content = new FormUrlEncodedContent(
                parameters.Select(item =>
                    new KeyValuePair<string, string>(
                            item.Key,
                            item.Value)));
        }
    }
}
