/*
 * WARNING: This is generated code. Any changes you make will
 * be overwritten. If you wish to modify this class create a
 * partial definition in a seperate file.
 *
 */
using System;
using System.Collections.Generic;
using Aggregator.Framework.Enums;

namespace Aggregator.Infrastructure.Services
{
	[Aggregator.Framework.Metadata.CredentialType(typeof(Aggregator.Infrastructure.Credentials.OAuth2Credentials))]        
	public partial class Facebook : OAuth2Service
	{
		public override Uri AuthorizeUrl => new Uri("https://www.facebook.com/dialog/oauth");
		public override Uri CallbackUrl => new Uri("http://localhost:33533/facebook");
		public override Uri AccessUrl => new Uri("https://graph.facebook.com/oauth/access_token");
		public override String Scope => "user_activities,user_events,user_likes,user_friends,user_status,read_stream";
		public override String TokenName => "access_token";
		public override ResponseTypeEnum[] AuthorizationGrants => new ResponseTypeEnum[] {ResponseTypeEnum.Code, ResponseTypeEnum.Token};
	}
}
