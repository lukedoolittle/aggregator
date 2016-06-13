using System;
using BatmansBelt;
using Newtonsoft.Json.Linq;
using Aggregator.Test.Helpers;

namespace Aggregator.Test.Fixtures
{
    public class SampleJsonFixture : IDisposable
    {
        public JToken SampleJson { get; }

        public SampleJsonFixture()
        {
            new GlobalizationFixture();

            SampleJson = ManifestResource
                .GetResourceAsObject<JToken>(
                    "Aggregator.Test.sampleobjects.json",
                    GetType().Assembly);
        }

        public void Dispose()
        {
        }
    }
}
