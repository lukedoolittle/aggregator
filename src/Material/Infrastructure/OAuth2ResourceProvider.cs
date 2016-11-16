using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Foundations.Extensions;
using Foundations.HttpClient.Enums;
using Material.Exceptions;

namespace Material.Infrastructure
{
    //TODO: should override GetHashCode() for this value object
    public abstract class OAuth2ResourceProvider : ResourceProvider
    {
        public virtual Uri AuthorizationUrl { get; }
        public abstract Uri TokenUrl { get; }
        public abstract List<string> AvailableScopes { get; }
        public List<string> Scopes { get; } = new List<string>();
        public virtual string Scope => string.Join(ScopeDelimiter.ToString(), Scopes);
        public virtual char ScopeDelimiter => ' ';
        public virtual string TokenName => OAuth2Parameter.BearerHeader.EnumToString();
        public virtual Dictionary<string, string> Parameters { get; } = 
            new Dictionary<string, string>();
        public virtual Dictionary<HttpRequestHeader, string> Headers { get; } = 
            new Dictionary<HttpRequestHeader, string>();
        public abstract List<OAuth2FlowType> Flows { get; }
        public virtual OAuth2FlowType Flow { get; private set; }
        public abstract List<GrantType> GrantTypes { get; }
        public virtual List<OAuth2ResponseType> AllowedResponseTypes { get; } = 
            new List<OAuth2ResponseType>();

        public virtual OAuth2ResponseType ResponseType
        {
            get { throw new NotImplementedException(); }
        }

        public virtual OAuth2ResourceProvider SetFlow(OAuth2FlowType flow)
        {
            if (!Flows.Contains(flow))
            {
                throw new InvalidFlowTypeException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        StringResources.FlowTypeNotSupportedException,
                        GetType().Name));
            }

            Flow = flow;

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
                if (!AvailableScopes.Contains(scope))
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
            }

            return this;
        }
    }
}
