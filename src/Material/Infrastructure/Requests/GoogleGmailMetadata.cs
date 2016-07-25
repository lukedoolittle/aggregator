using Material.Metadata;
using Material.Infrastructure.ProtectedResources;
using System;
using System.Collections.Generic;
using Material.Enums;
using Material.Infrastructure;

namespace Material.Infrastructure.Requests
{     
    /// <summary>
    /// Lists the messages in the user's mailbox
    /// </summary>
    [ServiceType(typeof(Google))]
	public partial class GoogleGmailMetadata : OAuthRequest              
	{
        public override String Host => "https://www.googleapis.com";
        public override String Path => "/gmail/v1/users/me/messages";
        public override String HttpMethod => "GET";
        public override List<String> RequiredScopes => new List<String> { "https://www.googleapis.com/auth/gmail.readonly" };
        /// <summary>
        /// Only return messages matching the specified query
        /// </summary>
        [Name("q")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String Q { get; set; }
        /// <summary>
        /// Maximum number of messages to return
        /// </summary>
        [Name("maxResults")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Int32> MaxResults { get; set; }
        /// <summary>
        /// Page token to retrieve a specific page of results in the list
        /// </summary>
        [Name("pageToken")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String PageToken { get; set; }
        /// <summary>
        /// Include messages from SPAM and TRASH in the results
        /// </summary>
        [Name("includeSpamTrash")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  Nullable<Boolean> IncludeSpamTrash { get; set; }
        /// <summary>
        /// Only return messages with labels that match all of the specified label IDs
        /// </summary>
        [Name("labelIds")]
        [ParameterType(RequestParameterTypeEnum.Query)]
        public  String LabelIds { get; set; }
	}
}
