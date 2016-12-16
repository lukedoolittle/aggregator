using Material.Infrastructure.Requests;
using Material.OAuth.AuthenticatorParameters;

namespace Material.Infrastructure.ProtectedResources
{
    public partial class Fitbit
    { 
        public override void SetClientProperties(
            string clientId, 
            string clientSecret)
        {
            //Fitbit requires that you add the OAuth2 Client Credentials grant type
            //header even during an auth code workflow
            base.SetClientProperties(clientId, clientSecret);

            var credentials = new OAuth2ClientCredentials(
                clientId, 
                clientSecret);

            Headers.Add(
                credentials.HeaderName, 
                credentials.Value);
        }

        public override string Scope
        {
            get
            {
                //Fitbit requires that you have at least one scope and
                //profile is the most basic
                if (Scopes == null || Scopes.Count == 0)
                {
                    AddRequestScope<FitbitProfile>();
                }

                return base.Scope;
            }
        }
    }
}
