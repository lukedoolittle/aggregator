using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Material.Framework.Enums;
using Material.Framework.Exceptions;
using Material.Framework.Extensions;

namespace Material.Domain.Core
{
    public abstract class OAuth2ResourceProvider : ResourceProvider
    {
        public virtual Uri AuthorizationUrl { get; }
        public abstract Uri TokenUrl { get; }
        public abstract List<string> AvailableScopes { get; }
        public List<string> Scopes { get; } = new List<string>();
        public virtual string Scope => string.Join(ScopeDelimiter.ToString(), Scopes);
        public virtual char ScopeDelimiter => ' ';
        public virtual string TokenName => OAuth2Parameter.BearerHeader.EnumToString();
        public Dictionary<string, string> Parameters { get; } = 
            new Dictionary<string, string>();
        public Dictionary<HttpRequestHeader, string> Headers { get; } = 
            new Dictionary<HttpRequestHeader, string>();

        public virtual bool SupportsPkce { get; } = false;
        public virtual bool SupportsCustomUrlScheme { get; } = false;

        public abstract List<OAuth2FlowType> AllowedFlows { get; }
        public OAuth2FlowType Flow { get; protected set; }

        public abstract List<GrantType> AllowedGrantTypes { get; }
        public GrantType Grant { get; protected set; }

        public virtual List<OAuth2ResponseType> AllowedResponseTypes { get; } = 
            new List<OAuth2ResponseType>();
        public OAuth2ResponseType ResponseType { get; protected set; }

        public virtual OAuth2ResourceProvider SetGrant(GrantType grantType)
        {
            if (!AllowedGrantTypes.Contains(grantType))
            {
                throw new InvalidGrantTypeException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResources.GrantTypeNotSupportedException,
                        grantType.EnumToString(),
                        GetType().Name));
            }

            Grant = grantType;

            return this;
        }

        public virtual OAuth2ResourceProvider SetResponse(OAuth2ResponseType response)
        {
            if (!AllowedResponseTypes.Contains(response))
            {
                throw new InvalidResponseTypeException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResources.ResponseTypeNotSupported,
                        response.EnumToString(),
                        GetType().Name));
            }

            ResponseType = response;

            return this;
        }

        public virtual OAuth2ResourceProvider SetFlow(OAuth2FlowType flow)
        {
            if (!AllowedFlows.Contains(flow))
            {
                throw new InvalidFlowTypeException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResources.FlowTypeNotSupported,
                        flow.EnumToString(),
                        GetType().Name));
            }

            Flow = flow;

            return this;
        }

        public virtual OAuth2ResourceProvider SetCustomUrlBrowsingParameters(
            Uri callbackUri)
        {
            return this;
        }

        public virtual void SetClientProperties(
            string clientId,
            string clientSecret)
        { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public OAuth2ResourceProvider AddRequestScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            var request = new TRequest();

            foreach (var scope in request.RequiredScopes)
            {
                AddRequestScope(scope);
            }

            return this;
        }

        public OAuth2ResourceProvider AddRequestScope(string scope)
        {
            if (!AvailableScopes.Contains(scope) && 
                QuantfabricConfiguration.StrictScoping)
            {
                throw new InvalidScopeException(string.Format(
                    CultureInfo.InvariantCulture,
                    StringResources.ScopeException,
                    scope,
                    this.GetType().Name));
            }
            if (!Scopes.Contains(scope))
            {
                Scopes.Add(scope);
            }

            return this;
        }
    }
}
