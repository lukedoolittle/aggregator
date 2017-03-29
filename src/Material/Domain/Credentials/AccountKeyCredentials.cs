namespace Material.Domain.Credentials
{
    public class AccountKeyCredentials : TokenCredentials
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public string AccountName { get; private set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public string AccountKey { get; private set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public string AccountKeyType { get; private set; }

        public override string ExpiresIn => "0";
        public override bool AreValidIntermediateCredentials => true;

        public AccountKeyCredentials AddAccountInformation(
            string name, 
            string key, 
            string keyType)
        {
            AccountName = name;
            AccountKey = key;
            AccountKeyType = keyType;

            return this;
        }

    }
}
