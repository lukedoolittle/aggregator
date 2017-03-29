using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.HttpClient;
using Material.HttpClient.Authenticators;
using Material.HttpClient.Canonicalizers;
using Material.HttpClient.Extensions;
using Material.Workflow.AuthenticatorParameters;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    [Trait("Category", "Continuous")]
    public class CanonicalizationTests
    {
        [Fact]
        public void CanonicalizeOAuth1Request()
        {
            var expected = "GET&https%3A%2F%2Fapi.twitter.com%2F1.1%2Faccount%2Fverify_credentials.json&oauth_consumer_key%3DconsumerKey%26oauth_nonce%3Dnonce%26oauth_signature_method%3DsigningAlgorithm%26oauth_timestamp%3D1486376100%26oauth_token%3DoauthToken%26oauth_version%3D1.0";

            var baseAddress = "https://api.twitter.com/";
            var path = "/1.1/account/verify_credentials.json";
            var consumerKey = "consumerKey";
            var oauthToken = "oauthToken";
            var signingAlgorithm = "signingAlgorithm";
            var timestamp = new DateTime(2017, 02, 6, 10, 15, 00);
            var nonce = "nonce";

            var parameters = new List<IAuthenticatorParameter>
            {
                new OAuth1ConsumerKey(consumerKey),
                new OAuth1Token(oauthToken),
                new OAuth1Timestamp(timestamp),
                new OAuth1Nonce(nonce),
                new OAuth1Version(),
                new OAuth1SignatureMethod(signingAlgorithm)
            };

            var requestBuilder = new HttpRequestBuilder(baseAddress)
                .GetFrom(path);

            foreach (var parameter in parameters)
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

            var canonicalizer = new OAuth1Canonicalizer();

            var actual = canonicalizer.CanonicalizeHttpRequest(requestBuilder);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanonicalizeMicrosoftRequest()
        {
            var expected = "GET\n\n\nSun, 11 Oct 2009 21:49:13 GMT\n/MyAccount/Tables()";
            var accountName = "MyAccount";
            var baseAddress = "https://myaccount.table.core.windows.net/";
            var path = "Tables()";
            var timestamp = new DateTime(2009, 10, 11, 21, 49, 13, DateTimeKind.Utc)
                .ToString("R", CultureInfo.InvariantCulture);

            var request = new HttpRequestBuilder(baseAddress)
                .GetFrom(path)
                .Header(
                    HttpRequestHeader.ContentType, 
                    MediaType.Json.EnumToString())
                .Header("x-ms-date", timestamp)
                .Parameter("timeout", 20.ToString());

            var canonicalizer = new MicrosoftCanonicalizer(accountName);

            var actual = canonicalizer.CanonicalizeHttpRequest(request);

            Assert.Equal(expected, actual);
        }
    }
}
