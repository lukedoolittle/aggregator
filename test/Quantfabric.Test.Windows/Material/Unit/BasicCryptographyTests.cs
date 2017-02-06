using System;
using Foundations.HttpClient.Cryptography.Algorithms;
using Foundations.HttpClient.Cryptography.Enums;
using Foundations.HttpClient.Cryptography.Keys;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    [Trait("Category", "Continuous")]
    public class BasicCryptographyTests
    {
        [Fact]
        public void PerformHmacSha256Hash()
        {
            var expected = "6Fqo12yiaPB67N20w+b38xgXyazCg06fMSgXvNxqOx0=";

            var message = "GET\n\n\nFri, 03 Feb 2017 21:45:34 GMT\n/MyAccount/sportingproducts(PartitionKey='Baseball',RowKey='BBt1032')";
            var key = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";

            var signature = HmacDigestSigningAlgorithm.Sha256Algorithm().SignMessage(
                message,
                new HashKey(
                    key, 
                    StringEncoding.Base64));

            var actual = Convert.ToBase64String(signature);

            Assert.Equal(expected, actual);
        }
    }
}
