using System.Collections.Generic;
using System.Net;
using BatmansBelt;
using BatmansBelt.Metadata;
using BatmansBelt.Serialization;
using Couchbase.Management;
using Aggregator.Framework.Serialization;

namespace Aggregator.Configuration.Startup
{
    [Dependency(typeof(SerializationStartupTask))]
    public class CouchbaseConfigurationTask : IStartupTask
    {
        private readonly string _configurationSection;
        private readonly string _bucketName;
        private readonly string _designDocumentName;
        private readonly string _viewName;
        private readonly string _viewMapFunction;

        public CouchbaseConfigurationTask()
        {
            _configurationSection = ApplicationSettings.CouchbaseConfigSection;
            _bucketName = ApplicationSettings.CouchbaseBucket;
            _designDocumentName = ApplicationSettings.CouchbaseDesignDoc;
            _viewName = ApplicationSettings.CouchbaseView;
            _viewMapFunction = ApplicationSettings.CouchbaseMapWindows;
        }

        public void Execute()
        {
            var cluster = new CouchbaseCluster(_configurationSection);

            CouchbaseDesignDocument designDoc;
            var documentExists = false;

            try
            {
                designDoc = cluster
                    .RetrieveDesignDocument(
                        _bucketName,
                        _designDocumentName)
                    .AsEntity<CouchbaseDesignDocument>(false);
                documentExists = true;
            }
            catch (WebException)
            {
                designDoc = new CouchbaseDesignDocument
                {
                    options =
                    {
                        updateMinChanges = 10,
                        replicaUpdateMinChanges = 10,
                        updateInterval = 60000
                    }
                };
            }

            if (!designDoc.views.ContainsKey(_viewName))
            {
                var viewMap = new MapFunction {map = _viewMapFunction};
                designDoc.views.Add(_viewName, viewMap);

                if (documentExists)
                {
                    cluster.DeleteDesignDocument(
                        _bucketName,
                        _designDocumentName);
                }
                cluster.CreateDesignDocument(
                    _bucketName,
                    _designDocumentName,
                    designDoc.AsJson(false));
            }
        }
    }

    public class CouchbaseDesignDocument
    {
        public Dictionary<string, MapFunction> views { get; set; }

        public DesignOptions options { get; set; }

        public CouchbaseDesignDocument()
        {
            views = new Dictionary<string, MapFunction>();
            options = new DesignOptions();
        }
    }

    public class MapFunction
    {
        public string map { get; set; }
    }

    public class DesignOptions
    {
        public int updateMinChanges { get; set; }

        public int updateInterval { get; set; }

        public int replicaUpdateMinChanges { get; set; }

    }
}
