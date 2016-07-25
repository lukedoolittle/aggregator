using System;
using System.Collections.Generic;
using System.Net;
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


        public virtual void SetClientProperties(
            string clientId,
            string clientSecret)
        {
            Flow = string.IsNullOrEmpty(clientSecret) ? 
                ResponseTypeEnum.Token : 
                ResponseTypeEnum.Code;

            if (!Flows.Contains(Flow))
            {
                throw new GrantTypeException(
                    string.Format(
                        StringResources.GrantTypeNotSupportedException,
                        GetType().Name));
            }
        }

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
