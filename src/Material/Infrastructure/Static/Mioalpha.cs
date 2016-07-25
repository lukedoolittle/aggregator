using Material.Infrastructure.Credentials;
using Material.Metadata;

namespace Material.Infrastructure.ProtectedResources
{
	[CredentialType(typeof(BluetoothCredentials))]        
	public partial class Mioalpha : BluetoothResourceProvider
	{
	}
}
