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
	public partial class Runkeeper : OAuth2Service
	{
		public override Uri AuthorizeUrl => new Uri("https://runkeeper.com/apps/authorize");
		public override Uri CallbackUrl => new Uri("http://localhost:33533/runkeeper");
		public override Uri AccessUrl => new Uri("https://runkeeper.com/apps/token");
		public override String Scope => "";
		public override String TokenName => "Bearer";
		public override ResponseTypeEnum[] AuthorizationGrants => new ResponseTypeEnum[] {ResponseTypeEnum.Code};
	}
}
