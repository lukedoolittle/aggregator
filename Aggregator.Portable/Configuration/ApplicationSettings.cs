namespace Aggregator.Configuration
{
    public static class ApplicationSettings
    {
        public static string ServerUrl => "http://localhost:3000";
        public static string CouchbaseConfigSection => "couchbase";
        public static string CouchbaseBucket => "default";
        public static string CouchbaseDesignDoc => "Aggregator";
        public static string CouchbaseView => "get_all";
        public static string CouchbaseMapWindows => "function (doc, meta) { if (doc.AggregateId && doc.Type) emit(doc.AggregateId + doc.Type, null); }";
    }
}
