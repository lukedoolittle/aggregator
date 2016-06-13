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
	public partial class Google : OAuth2Service
	{
		public override Uri AuthorizeUrl => new Uri("https://accounts.google.com/o/oauth2/auth");
		public override Uri CallbackUrl => new Uri("http://localhost:33533/google");
		public override Uri AccessUrl => new Uri("https://accounts.google.com/o/oauth2/token");
		public override String Scope => "https://www.googleapis.com/auth/gmail.readonly";
		public override String TokenName => "Bearer";
		public override ResponseTypeEnum[] AuthorizationGrants => new ResponseTypeEnum[] {ResponseTypeEnum.Code, ResponseTypeEnum.Token};
		public override Dictionary<String,String> Parameters => new Dictionary<String,String>{{"access_type", "offline"}};
	}
}
