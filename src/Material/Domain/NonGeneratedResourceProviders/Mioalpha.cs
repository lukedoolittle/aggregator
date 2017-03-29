using Material.Domain.Credentials;
using Material.Framework.Metadata;

namespace Material.Domain.ResourceProviders
{
	[CredentialType(typeof(BluetoothCredentials))]        
	public partial class Mioalpha : BluetoothResourceProvider
	{
	}
}
