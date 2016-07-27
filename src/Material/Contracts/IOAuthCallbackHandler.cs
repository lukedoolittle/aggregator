using System;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthCallbackHandler
    {
        TToken ParseAndValidateCallback<TToken>(
            Uri responseUri)
            where TToken : TokenCredentials;
    }
}
