#if __ANDROID__
using System;
using Material.Adapters;
using Material.Infrastructure.Static;

namespace Material
{
    using System.Threading.Tasks;

    public class SMSRequester
    {
        public async Task<SMSResponse> MakeSMSRequest(DateTime startTimeFilter = default(DateTime))
        {
            var result = await new AndroidSMSAdapter()
                .GetAllSMS(startTimeFilter)
                .ConfigureAwait(false);

            var response = new SMSResponse();
            response.AddRange(result);
            return response;
        }
    }
}
#endif
