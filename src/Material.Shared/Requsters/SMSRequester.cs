#if __ANDROID__
using System;
using Material.Domain.Responses;
using Material.Permissions;

namespace Material
{
    using System.Threading.Tasks;

    public class SMSRequester
    {
        /// <summary>
        /// Get all SMS from sent and inbox
        /// </summary>
        /// <returns>List of SMS items</returns>
        public Task<SMSResponse> MakeSMSRequestAsync()
        {
            return MakeSMSRequestAsync(
                default(DateTime),
                false);
        }

        /// <summary>
        /// Get all SMS from sent and inbox
        /// </summary>
        /// <param name="skipSMSAuthorization">If true skips device authorization for SMS</param>
        /// <returns>List of SMS items</returns>
        public Task<SMSResponse> MakeSMSRequestAsync(
            bool skipSMSAuthorization)
        {
            return MakeSMSRequestAsync(
                default(DateTime), 
                skipSMSAuthorization);
        }

        /// <summary>
        /// Get all SMS from sent and inbox
        /// </summary>
        /// <param name="startTimeFilter">Oldest SMS to get from device</param>
        /// <returns>List of SMS items</returns>
        public Task<SMSResponse> MakeSMSRequestAsync(
            DateTime startTimeFilter)
        {
            return MakeSMSRequestAsync(
                startTimeFilter, 
                false);
        }

        /// <summary>
        /// Get all SMS from sent and inbox
        /// </summary>
        /// <param name="startTimeFilter">Oldest SMS to get from device</param>
        /// <param name="skipSMSAuthorization">If true skips device authorization for SMS</param>
        /// <returns>List of SMS items</returns>
        public async Task<SMSResponse> MakeSMSRequestAsync(
            DateTime startTimeFilter,
            bool skipSMSAuthorization)
        {
            if (!skipSMSAuthorization)
            {
                var authorizationResult = await new DeviceAuthorizationFacade()
                    .AuthorizeSMS()
                    .ConfigureAwait(false);
            }

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
