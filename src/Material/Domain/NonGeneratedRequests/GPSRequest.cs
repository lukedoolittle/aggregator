using Material.Domain.Core;
using Material.Domain.ResourceProviders;
using Material.Framework.Metadata;

namespace Material.Domain.Requests
{
    [ServiceType(typeof(Device))]        
	public partial class GPSRequest : DeviceRequest
	{
	}
}
