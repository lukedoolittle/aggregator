using System;
using System.Collections.Generic;
using Material.Domain.Core;

namespace Material.Domain.ResourceProviders
{
    public partial class Spotify
    {
        private readonly Dictionary<string, string> _mobileParameters = 
            new Dictionary<string, string>
            {
                {"utm_source", "spotify-sdk"},
                {"utm_medium", "android-sdk"}
            };

        public override OAuth2ResourceProvider SetCustomUrlBrowsingParameters(
            Uri callbackUri)
        {
            //Spotify needs a couple of extra parameters in order to accept a
            //protocal based redirect_uri parameter
            foreach (var parameter in _mobileParameters)
            {
                Parameters.Add(
                    parameter.Key, 
                    parameter.Value);
            }

            return base.SetCustomUrlBrowsingParameters(callbackUri);
        }
    }
}
