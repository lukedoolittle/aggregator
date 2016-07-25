using System.Net;
using Foundations.Extensions;
using Material.Enums;

namespace Material.Infrastructure.ProtectedResources
{
    public partial class Fitbit
    {
        public override void SetClientProperties(
            string clientId, 
            string clientSecret)
        {
            base.SetClientProperties(clientId, clientSecret);

            var key = $"{clientId}:{clientSecret}".ToBase64String();
            Headers.Add(
                HttpRequestHeader.Authorization,
                $"{OAuth2ParameterEnum.BasicHeader.EnumToString()} {key}");
        }
    }
}
