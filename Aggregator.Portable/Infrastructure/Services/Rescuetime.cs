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
	public partial class Rescuetime : OAuth2Service
	{
		public override Uri AuthorizeUrl => new Uri("https://www.rescuetime.com/oauth/authorize");
		public override Uri CallbackUrl => new Uri("https://localhost:33533/rescuetime");
		public override Uri AccessUrl => new Uri("https://www.rescuetime.com/oauth/token");
		public override String Scope => "time_data";
		public override String TokenName => "access_token";
		public override ResponseTypeEnum[] AuthorizationGrants => new ResponseTypeEnum[] {ResponseTypeEnum.Code};
	}
}
