using System;
using System.Collections.Generic;

namespace Foundations.HttpClient.Authenticators
{
    public class AuthenticatorBuilder : IAuthenticator
    {
        private readonly List<IAuthenticatorParameter> _parameters = 
            new List<IAuthenticatorParameter>();

        public AuthenticatorBuilder AddParameter(
            IAuthenticatorParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            _parameters.Add(parameter);

            return this;
        }

        void IAuthenticator.Authenticate(
            HttpRequestBuilder requestBuilder)
        {
            if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

            foreach (var parameter in _parameters)
            {
                requestBuilder.Parameter(
                    parameter.Name, 
                    parameter.Value);
            }
        }
    }
}
