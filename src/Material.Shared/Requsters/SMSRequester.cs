﻿#if __ANDROID__
using System;
using Material.Adapters;
using Material.Infrastructure.Requests;

namespace Material
{
    using System.Threading.Tasks;

    public class SMSRequester
    {
        /// <summary>
        /// Get all SMS from sent and inbox
        /// </summary>
        /// <param name="startTimeFilter">Oldest SMS to get from device</param>
        /// <returns>List of SMS items</returns>
        public async Task<SMSResponse> MakeSMSRequestAsync(
            DateTime startTimeFilter = default(DateTime))
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
