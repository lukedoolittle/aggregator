using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Gets the specified message
    /// </summary>
    [ServiceType(typeof(Google))]
	public partial class GoogleGmail : OAuthRequest              
	{
        public override String Host => "https://www.googleapis.com";
        public override String Path => "/gmail/v1/users/me/messages/{messageId}";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String> { "https://www.googleapis.com/auth/gmail.readonly" };
        /// <summary>
        /// The ID of the message to retrieve
        /// </summary>
        [Name("messageId")]
        [ParameterType(RequestParameterTypeEnum.Path)]
        public  String MessageId { get; set; }
	}
}
