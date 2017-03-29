using System;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient.Authenticators;

namespace Material.Workflow.AuthenticatorParameters
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
