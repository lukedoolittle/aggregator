using System;
using Material.Infrastructure.Credentials;

namespace Aggregator.Test.Mocks
{
    public class CredentialMock : TokenCredentials
    {
        private string _expiresIn;

        public Guid Id { get; set; }
        public override bool HasValidPublicKey { get; }
        public override string ExpiresIn => _expiresIn;
        public override bool AreValidIntermediateCredentials { get; }

        public void Expire()
        {
            _expiresIn = "1";
        }
    }
}
