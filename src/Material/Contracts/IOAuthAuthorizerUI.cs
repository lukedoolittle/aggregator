﻿using System;
using System.Threading.Tasks;
using Material.Enums;
using Material.Infrastructure.Credentials;

namespace Material.Contracts
{
    public interface IOAuthAuthorizerUI<TCredentials>
        where TCredentials : TokenCredentials
    {
        AuthenticationInterface BrowserType { get; }

        Task<TCredentials> Authorize(
            Uri authorizationUri,
            string userId);
    }
}
