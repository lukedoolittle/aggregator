using System;
using System.Collections.Generic;
using System.Linq;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Infrastructure;

namespace CodeGen
{
    public class BoxedOAuth2ResourceProvider
    {
        public string Name { get; }
        public string Comments { get; }
        public List<string> AvailableScopes { get; }
        public List<OAuth2FlowType> AllowedFlows { get; } = 
            new List<OAuth2FlowType>();
        public List<GrantType> AllowedGrantTypes { get; }
        public List<OAuth2ResponseType> AllowedResponseTypes { get; }
        public string TokenName { get; }
        public Uri AuthorizationUrl { get; }
        public Uri TokenUrl { get; private set; }
        private bool _supportsPkce;
        public bool SupportsPkce => _supportsPkce;
        private bool _supportsCustomUrlScheme;
        public bool SupportsCustomUrlScheme => _supportsCustomUrlScheme;
        public char ScopeDelimiter { get; }

        public BoxedOAuth2ResourceProvider(
            string name,
            string comments,
            IList<string> availableScopes,
            string flow,
            IList<string> allowedGrantTypes,
            IList<string> allowedResponseTypes,
            string tokenName,
            string authorizationUrl,
            string tokenUrl,
            bool supportsPkce,
            bool supportsCustomUrlScheme,
            string scopeDelimiter)
        {
            Name = name;
            Comments = comments;
            AvailableScopes = availableScopes?.ToList() ?? new List<string>();
            if (flow != null)
            {
                AllowedFlows.Add(flow.StringToEnum<OAuth2FlowType>());
            }
            AllowedGrantTypes = allowedGrantTypes?.Select(g => g.StringToEnum<GrantType>()).ToList() ?? 
                new List<GrantType>(); 
            AllowedResponseTypes = allowedResponseTypes?.Select(r => r.StringToEnum<OAuth2ResponseType>()).ToList() ?? 
                new List<OAuth2ResponseType>();
            TokenName = tokenName;
            if (authorizationUrl != null)
            {
                AuthorizationUrl = new Uri(authorizationUrl);
            }
            if (tokenUrl != null)
            {
                TokenUrl = new Uri(tokenUrl);
            }
            _supportsPkce = supportsPkce;
            _supportsCustomUrlScheme = supportsCustomUrlScheme;
            ScopeDelimiter = scopeDelimiter?[0] ?? ' ';
        }

        public string FormattedScopes => PrintingFormatter.FormatStringList(AvailableScopes);
        public string FormattedFlows => PrintingFormatter.FormatEnumList(AllowedFlows);
        public string FormattedGrantTypes => PrintingFormatter.FormatEnumList(AllowedGrantTypes);
        public string FormattedResponseTypes => PrintingFormatter.FormatEnumList(AllowedResponseTypes);

        public void AddPkceSupport(bool supportsPkce)
        {
            _supportsPkce = _supportsPkce | supportsPkce;
        }

        public void AddCustomUrlSupport(bool supportsCustomUrlScheme)
        {
            _supportsCustomUrlScheme = _supportsCustomUrlScheme | supportsCustomUrlScheme;
        }

        public BoxedOAuth2ResourceProvider Merge(BoxedOAuth2ResourceProvider instance)
        {
            TokenUrl = TokenUrl ?? instance.TokenUrl;
            AddPkceSupport(instance.SupportsPkce);
            AddCustomUrlSupport(instance.SupportsCustomUrlScheme);
            AllowedFlows.AddRange(instance.AllowedFlows);
            AllowedGrantTypes.AddRange(instance.AllowedGrantTypes);
            AllowedResponseTypes.AddRange(instance.AllowedResponseTypes);

            return this;
        }

        public BoxedOpenIdResourceProvider Merge(BoxedOpenIdResourceProvider instance)
        {
            return instance.Merge(this);
        }
    }
}
