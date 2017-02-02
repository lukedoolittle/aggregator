using System;

namespace Material.Infrastructure.Credentials
{
    public class AccountKeyCredentials : TokenCredentials
    {
        public string AccountName { get; private set; }
        public string AccountKey { get; private set; }
        public string AccountKeyType { get; private set; }

        public override string ExpiresIn => "0";
        public override bool AreValidIntermediateCredentials => true;
    }
}
