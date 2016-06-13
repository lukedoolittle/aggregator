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
	public partial class Spotify : OAuth2Service
	{
		public override Uri AuthorizeUrl => new Uri("https://accounts.spotify.com/authorize");
		public override Uri CallbackUrl => new Uri("http://localhost:33533/spotify");
		public override Uri AccessUrl => new Uri("https://accounts.spotify.com/api/token");
		public override String Scope => "playlist-read-private user-follow-read user-library-read";
		public override String TokenName => "Bearer";
		public override ResponseTypeEnum[] AuthorizationGrants => new ResponseTypeEnum[] {ResponseTypeEnum.Code, ResponseTypeEnum.Token};
	}
}
