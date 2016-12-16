using System;
using Foundations.Extensions;
using Foundations.HttpClient.Authenticators;
using Foundations.HttpClient.Enums;

namespace Material.OAuth.AuthenticatorParameters
{
    public class OAuth2CodeChallengeMethod : IAuthenticatorParameter
    {
        public string Name => OAuth2Parameter.ChallengeMethod.EnumToString();
        public string Value { get; }
        public HttpParameterType Type => HttpParameterType.Unspecified;

        public OAuth2CodeChallengeMethod(string challengeMethod)
        {
            if (challengeMethod == null) throw new ArgumentNullException(nameof(challengeMethod));

            Value = challengeMethod;
        }

        public OAuth2CodeChallengeMethod(CodeChallengeMethod method) : 
            this(method.EnumToString())
        { }
    }
}
