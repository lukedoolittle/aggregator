namespace Material.Framework
{
    public sealed class Platform
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
    }
}
