using System.Linq;
using CodeGen;
using Xunit;

namespace Foundations.Test
{
    public class CodeGenTests
    {
        [Fact]
        public void CreateServiceAndRequestClassesHaveProducesAndConsumes()
        {
            var serviceNamespace = "SampleApiNamespace.Services";
            var requestNamespace = "SampleApiNamespace.Requests";

            var codeGen = new SwaggerToClass("sampleapi.json");

            var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

            var requestClasses = codeGen.GenerateRequestClasses(
                requestNamespace, 
                serviceClass.Name, 
                serviceNamespace);

            foreach (var requestClass in requestClasses)
            {
                var produces = requestClass
                    .Properties
                    .FirstOrDefault(p => p.Name == "Produces");

                Assert.NotNull(produces);

                var consumes = requestClass
                    .Properties
                    .FirstOrDefault(p => p.Name == "Consumes");

                Assert.NotNull(consumes);
            }
        }

        [Fact]
        public void CreateServiceAndRequestClassesHasResponseCode()
        {
            var serviceNamespace = "SampleApiNamespace.Services";
            var requestNamespace = "SampleApiNamespace.Requests";

            var codeGen = new SwaggerToClass("sampleapi.json");

            var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

            var requestClasses = codeGen.GenerateRequestClasses(
                requestNamespace,
                serviceClass.Name,
                serviceNamespace);

            foreach (var requestClass in requestClasses)
            {
                var expects = requestClass
                    .Properties
                    .FirstOrDefault(p => p.Name == "ExpectedStatusCodes");

                Assert.NotNull(expects);
            }
        }
    }
}
