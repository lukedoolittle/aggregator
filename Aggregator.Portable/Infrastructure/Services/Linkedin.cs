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
	public partial class Linkedin : OAuth2Service
	{
		public override Uri AuthorizeUrl => new Uri("https://www.linkedin.com/uas/oauth2/authorization");
		public override Uri CallbackUrl => new Uri("http://localhost:33533/linkedin");
		public override Uri AccessUrl => new Uri("https://www.linkedin.com/uas/oauth2/accessToken");
		public override String Scope => "r_basicprofile w_share r_emailaddress rw_company_admin";
		public override String TokenName => "Bearer";
		public override ResponseTypeEnum[] AuthorizationGrants => new ResponseTypeEnum[] {ResponseTypeEnum.Code};
	}
}
