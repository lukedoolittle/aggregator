using Material.Framework.Enums;

namespace Material.Domain.Requests
{
    public partial class WithingsWeighIn
    {
        public override void AddUserIdParameter(string userId)
        {
            Userid = userId;
            OverriddenResponseMediaType = MediaType.Json;
        }
    }
}
