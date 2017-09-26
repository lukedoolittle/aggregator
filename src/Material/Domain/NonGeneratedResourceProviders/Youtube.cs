using System.Collections.Generic;
using Material.Domain.Core;
using Material.Framework.Enums;
using Material.Framework.Extensions;

namespace Material.Domain.ResourceProviders
{
    public partial class Youtube
    {
        private readonly KeyValuePair<string, string> _accessType =
            new KeyValuePair<string, string>("access_type", "offline");

        public Youtube ForceConsent()
        {
            Parameters["prompt"] = GooglePrompt.Consent.EnumToString();

            return this;
        }

        public override OAuth2ResourceProvider SetFlow(OAuth2FlowType flow)
        {
            //When using the 'code' flow adding access_type=offline to the authentication 
            //uri results in a refresh token being returned. This is the behavior of every
            //other OAuth2 access code workflow by default so add this for consistency.
            //You cannot request an offline token using the 'token' flow
            switch (flow)
            {
                case OAuth2FlowType.Implicit:
                    Parameters.Remove(_accessType.Key);
                    break;
                case OAuth2FlowType.AccessCode:
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
