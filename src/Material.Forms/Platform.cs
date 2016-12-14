namespace Material.Framework
{
    public class Platform
    {
        private static volatile Platform _instance;
        private static readonly object _syncLock = new object();

        private Platform() { }

        public static Platform Current
        {
            get
            {
                if (_instance != null) return _instance;

                lock (_syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new Platform();
                    }
                }

                return _instance;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool CanProvideSecureBrowsing => false;
    }
}
