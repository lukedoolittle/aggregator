using System.Collections.Generic;
using Foundations.HttpClient.Enums;

namespace Material.Infrastructure.ProtectedResources
{
    public partial class Google
    {
        private readonly KeyValuePair<string, string> _accessType =
            new KeyValuePair<string, string>("access_type", "offline");


        public override OAuth2ResourceProvider SetFlow(ResponseTypeEnum flow)
        {
            //When using the 'code' flow adding access_type=offline to the authentication 
            //uri results in a refresh token being returned. This is the behavior of every
            //other OAuth2 access code workflow by default so add this for consistency.
            //You cannot request an offline token using the 'token' flow
            switch (flow)
            {
                case ResponseTypeEnum.Token:
                    Parameters.Remove(_accessType.Key);
                    break;
                case ResponseTypeEnum.Code:
                    if (!Parameters.ContainsKey(_accessType.Key))
                    {
                        Parameters.Add(
                            _accessType.Key,
                            _accessType.Value);
                    }
                    break;
            }

            return base.SetFlow(flow);
        }
    }
}
