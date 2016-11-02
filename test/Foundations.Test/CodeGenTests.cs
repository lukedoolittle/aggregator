using CodeGen;
using Xunit;

namespace Foundations.Test
{
    public class CodeGenTests
    {
        [Fact]
        public void CreateServiceAndRequestClasses()
        {
            var serviceNamespace = "SampleApiNamespace.Services";
            var requestNamespace = "SampleApiNamespace.Requests";

            var codeGen = new SwaggerToClass("sampleapi.json");

            var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

            var requestClasses = codeGen.GenerateRequestClasses(
                requestNamespace, 
                serviceClass.Name, 
                serviceNamespace);
        }
    }
}
