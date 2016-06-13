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
	public partial class Fitbit : OAuth2Service
	{
		public override Uri AuthorizeUrl => new Uri("https://www.fitbit.com/oauth2/authorize");
		public override Uri CallbackUrl => new Uri("http://localhost:33533/fitbit");
		public override Uri AccessUrl => new Uri("https://api.fitbit.com/oauth2/token");
		public override String Scope => "sleep activity heartrate";
		public override String TokenName => "Bearer";
		public override Boolean HasBasicAuthorization => true;
		public override ResponseTypeEnum[] AuthorizationGrants => new ResponseTypeEnum[] {ResponseTypeEnum.Code, ResponseTypeEnum.Token};
	}
}
