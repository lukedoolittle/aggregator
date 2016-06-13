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
	[Aggregator.Framework.Metadata.CredentialType(typeof(Aggregator.Infrastructure.Credentials.OAuth1Credentials))]        
	public partial class Fatsecret : OAuth1Service
	{
		public override Uri RequestUrl => new Uri("http://www.fatsecret.com/oauth/request_token");
		public override Uri AuthorizeUrl => new Uri("http://www.fatsecret.com/oauth/authorize");
		public override Uri AccessUrl => new Uri("http://www.fatsecret.com/oauth/access_token");
		public override Uri CallbackUrl => new Uri("http://localhost:33633/fatsecret");
	}
}
