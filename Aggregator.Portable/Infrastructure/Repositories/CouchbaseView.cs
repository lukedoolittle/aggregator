namespace Aggregator.Infrastructure.Repositories
{
    public class CouchbaseView
    {
        public CouchbaseView(
            string designDocument, 
            string viewName)
        {
            DesignDocument = designDocument;
            ViewName = viewName;
        }

        public string DesignDocument { get; }
        public string ViewName { get; }
    }
}
