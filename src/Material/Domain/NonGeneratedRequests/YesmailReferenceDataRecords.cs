namespace Material.Domain.Requests
{
    public partial class YesmailReferenceDataRecords
    {
        private bool _useTestEnvironment = false;

        public YesmailReferenceDataRecords UseTestEnvironment()
        {
            _useTestEnvironment = true;

            return this;
        }

        public override string GetModifiedHost()
        {
            return _useTestEnvironment ? 
                "https://api.test.yesmail.com" : 
                Host;
        }
    }
}
