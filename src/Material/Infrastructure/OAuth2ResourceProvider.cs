using System;
using System.Collections.Generic;
using System.Net;
using Foundations.HttpClient.Enums;
using Material.Enums;
using Material.Exceptions;

namespace Material.Infrastructure
{
    public abstract class OAuth2ResourceProvider : ResourceProvider
    {
        public abstract Uri AuthorizationUrl { get; }
        public abstract Uri TokenUrl { get; }
        public abstract List<string> AvailableScopes { get; }
        public List<string> Scopes { get; } = new List<string>();
        public virtual string Scope => string.Join(ScopeDelimiter.ToString(), Scopes);
        public virtual char ScopeDelimiter => ' ';
        public virtual string TokenName => "Bearer";
        public virtual Dictionary<string, string> Parameters { get; } = 
            new Dictionary<string, string>();
        public virtual Dictionary<HttpRequestHeader, string> Headers { get; } = 
            new Dictionary<HttpRequestHeader, string>();
        public abstract List<ResponseTypeEnum> Flows { get; }
        public virtual ResponseTypeEnum Flow { get; private set; }

        
        public virtual OAuth2ResourceProvider SetFlow(
            string clientId,
            string clientSecret,
            AuthenticationInterfaceEnum @interface,
            OAuthAppTypeEnum appType)
        {
            ResponseTypeEnum flow;

            if (@interface == AuthenticationInterfaceEnum.Dedicated
                && appType == OAuthAppTypeEnum.Desktop)
            {
                flow = clientSecret != null ?
                    ResponseTypeEnum.Code :
                    ResponseTypeEnum.Token;
            }
            else if (@interface == AuthenticationInterfaceEnum.Embedded
                     && appType == OAuthAppTypeEnum.Desktop)
            {
                throw new NotSupportedException(
                    StringResources.EmbeddedDesktopUINotSupportedException);
            }
            else if (@interface == AuthenticationInterfaceEnum.Dedicated
                     && appType == OAuthAppTypeEnum.Mobile)
            {
                flow = ResponseTypeEnum.Code;
            }
            else if (@interface == AuthenticationInterfaceEnum.Embedded
                     && appType == OAuthAppTypeEnum.Mobile)
            {
                flow = clientSecret != null ?
                    ResponseTypeEnum.Code :
                    ResponseTypeEnum.Token;
            }
            else
            {
                throw new NotSupportedException();
            }

            if (!Flows.Contains(flow))
            {
                throw new InvalidGrantTypeException(
                    string.Format(
                        StringResources.GrantTypeNotSupportedException,
                        GetType().Name));
            }

            Flow = flow;

            return this;
        }

        public virtual void SetClientProperties(
            string clientId,
            string clientSecret)
        { }

        public OAuth2ResourceProvider AddRequestScope<TRequest>()
            where TRequest : OAuthRequest, new()
        {
            var request = new TRequest();

            foreach (var scope in request.RequiredScopes)
            {
                if (!AvailableScopes.Contains(scope))
                {
                    throw new InvalidScopeException(string.Format(
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
