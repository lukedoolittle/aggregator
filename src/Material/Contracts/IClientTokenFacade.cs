﻿using System.Threading.Tasks;
using Material.Infrastructure;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IClientTokenFacade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        Task<OAuth2Credentials> GetClientAccessTokenCredentials<TResourceProvider>(
            string clientId,
            string clientSecret)
            where TResourceProvider : OAuth2ResourceProvider, new();
    }
}
