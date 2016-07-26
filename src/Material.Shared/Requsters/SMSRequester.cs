#if __ANDROID__
using Material.Adapters;
using Foundations.Serialization;

namespace Material
{
    using System.Threading.Tasks;

    public class SMSRequester
    {
        public static async Task<string> MakeSMSRequest()
        {
            var result = await new AndroidSMSAdapter()
                .GetAllSMS(null)
                .ConfigureAwait(false);

            return result.AsJson(false);
        }
    }
}
#endif
