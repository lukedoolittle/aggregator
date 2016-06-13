using System;
using Aggregator.Infrastructure.Credentials;

namespace Aggregator.Test.Mocks
{
    public class CredentialMock : TokenCredentials
    {
        private bool _isTokenExpired;

        public Guid Id { get; set; }
        public override bool HasValidProperties { get; }
        public override bool IsTokenExpired => _isTokenExpired;

        public void Expire()
        {
            _isTokenExpired = true;
        }
    }
}
