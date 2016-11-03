using Foundations.Enums;

namespace Material.Infrastructure.Requests
{
    public partial class WithingsWeighIn
    {
        public override void AddUserIdParameter(string userId)
        {
            Userid = userId;
            OverridenResponseMediaType = MediaType.Json;
        }
    }
}
