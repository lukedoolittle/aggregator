using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Material.Domain.Responses;

namespace Material.Contracts
{
    public interface ISMSAdapter
    {
        //Action<IEnumerable<Tuple<DateTimeOffset, JObject>>> Handler { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        Task<IEnumerable<SMSMessage>> GetAllSMS(DateTime filterDate);
    }
}
