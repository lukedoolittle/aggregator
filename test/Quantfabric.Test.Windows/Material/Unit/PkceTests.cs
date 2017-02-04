using System;
using System.Linq;
using Foundations.Extensions;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Enums;
using Material.OAuth.AuthenticatorParameters;
using Material.OAuth.Security;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    [Trait("Category", "Continuous")]
    public class PkceTests
    {
        [Fact]
        public void GenerateSha256PkceBundle()
        {
            var verifier = "dBjftJeZ4CVP-mB92K27uhbUJU1p1r_wW1gFWFOEjXk";
            var expected = "E9Melhoa2OwvFrEMTJguCHaoeK1t8URWbuGJSstw-cM";

            var bundle = new OAuth2Sha256PkceSecurityParameterBundle(
                DigestSigningAlgorithm.Sha256Algorithm());

            var securityStrategy = new OAuthSecurityStrategy(
                new InMemoryCryptographicParameterRepository(), 
                TimeSpan.FromMinutes(2));
            var userId = Guid.NewGuid().ToString();

            securityStrategy.SetSecureParameter(
                userId, 
                OAuth2Parameter.Verifier.EnumToString(), 
                verifier);
            var parameters = bundle.GetBundle(securityStrategy, userId);

            var actual = parameters.Single(p => p is OAuth2CodeChallenge).Value;

            Assert.Equal(expected, actual);
        }
    }
}
