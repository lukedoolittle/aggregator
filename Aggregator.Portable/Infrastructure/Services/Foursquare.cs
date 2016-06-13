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
	public partial class Foursquare : OAuth2Service
	{
		public override Uri AuthorizeUrl => new Uri("https://foursquare.com/oauth2/authorize");
		public override Uri CallbackUrl => new Uri("http://localhost:33533/foursquare");
		public override Uri AccessUrl => new Uri("https://foursquare.com/oauth2/access_token");
		public override String Scope => "";
		public override String TokenName => "oauth_token";
		public override ResponseTypeEnum[] AuthorizationGrants => new ResponseTypeEnum[] {ResponseTypeEnum.Code, ResponseTypeEnum.Token};
	}
}
