using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material.Infrastructure.Credentials
{
    public class CredentialMetadata<TCredentials>
        where TCredentials : TokenCredentials
    {
        public TCredentials Credentials { get; }
        public bool AreValidIntermediateCredentials { get; }
        public bool IsErrorResponse { get; }
        public string RequestId { get; }

        public bool ContainsResult => Credentials != null &&
                                      RequestId != null &&
                                      AreValidIntermediateCredentials;

        public CredentialMetadata(
            TCredentials credentials, 
            string requestId, 
            bool areValidIntermediateCredentials, 
            bool isErrorResponse)
        {
            Credentials = credentials;
            AreValidIntermediateCredentials = areValidIntermediateCredentials;
            RequestId = requestId;
            IsErrorResponse = isErrorResponse;
        }
    }
}
