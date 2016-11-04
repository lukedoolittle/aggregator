namespace Quantfabric.Test.Material.OAuthServer
{
    public class OAuth2Token
    {
        public string Code { get; private set; }
        public string AccessToken { get; private set; }
        public string Scope { get; private set; }

        public OAuth2Token(string code, string scope)
        {
            Code = code;
            Scope = scope;
        }

        public OAuth2Token(string scope)
        {
            Scope = scope;
        }

        public void SetCode(string code)
        {
            Code = code;
        }

        public void SetToken(string accessToken)
        {
            AccessToken = accessToken;
        }

        public void RemoveCode()
        {
            Code = null;
        }
    }
}
