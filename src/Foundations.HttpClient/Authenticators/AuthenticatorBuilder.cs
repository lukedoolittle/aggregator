using System;
using System.Collections.Generic;
using Foundations.HttpClient.Enums;

namespace Foundations.HttpClient.Authenticators
{
    public sealed class AuthenticatorBuilder : IAuthenticator
    {
        private readonly List<IAuthenticatorParameter> _parameters = 
            new List<IAuthenticatorParameter>();

        private IRequestSigningAlgorithm _signingAlgorithm;

        public AuthenticatorBuilder AddSigner(
            IRequestSigningAlgorithm signingAlgorithm)
        {
            if (signingAlgorithm == null) throw new ArgumentNullException(nameof(signingAlgorithm));

            _signingAlgorithm = signingAlgorithm;

            return this;
        }

        public AuthenticatorBuilder AddParameters(
            IEnumerable<IAuthenticatorParameter> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            foreach (var parameter in parameters)
            {
                AddParameter(parameter);
            }

            return this;
        }

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
                if (parameter.Type == HttpParameterType.Header)
                {
                    requestBuilder.Header(
                        parameter.Name, 
                        parameter.Value);
                }
                else
                {
                    requestBuilder.Parameter(
                        parameter.Name,
                        parameter.Value);
                }
            }

            _signingAlgorithm?.SignRequest(requestBuilder);
        }
    }
}
