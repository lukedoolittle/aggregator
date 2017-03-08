using System;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthCallbackHandler<TCredentials>
        where TCredentials : TokenCredentials
    {
        CredentialMetadata<TCredentials> ParseAndValidateCallback(
            Uri responseUri);
    }
}
