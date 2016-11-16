using System.Collections.Generic;
using System.Linq;
using CodeGen;
using Xunit;

namespace Foundations.Test
{
    [Trait("Category", "Continuous")]
    public class CodeGenTests
    {
        [Fact]
        public void CreateServiceAndRequestClassesCanPrettyPrint()
        {
            var serviceNamespace = "SampleApiNamespace.Services";
            var requestNamespace = "SampleApiNamespace.Requests";

            var codeGen = new SwaggerToClass("sampleapi.json");

            var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

            var requestClasses = codeGen.GenerateRequestClasses(
                requestNamespace,
                serviceClass.Name,
                serviceNamespace);

            var allClasses = new List<ClassRepresentation>
            {
                serviceClass
            };
            allClasses.AddRange(requestClasses);

            foreach (var myClass in allClasses)
            {
                Assert.NotNull(myClass.GetNamespaces());
                foreach (var metadata in myClass.Metadatas)
                {
                    Assert.NotNull(metadata.TypeName + metadata.ConstructorArguments);
                }
                foreach (var property in myClass.Properties)
                {
                    Assert.NotNull(property.Metadatas);
                    Assert.NotNull(property.AccessModifier);
                    Assert.NotNull(property.Modifier);
                    Assert.NotNull(property.TypeName);
                    Assert.NotNull(property.Name);
                    Assert.NotNull(property.Value);
                }
                foreach (var myEnum in myClass.Enums)
                {
                    foreach(var item in myEnum.Values)
                    {
                        Assert.NotNull(item.Key);
                        Assert.NotNull(item.Value);
                    }
                }
            }
        }

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

        [Fact]
        public void CreateServiceAndRequestClassesHasMultipleFlows()
        {
            var serviceNamespace = "SampleApiNamespace.Services";

            var codeGen = new SwaggerToClass("sampleapi.json");

            var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

            var flowProperty = serviceClass.
                Properties
                .FirstOrDefault(p => p.Name == "AllowedFlows");

            Assert.NotNull(flowProperty);

            var value = flowProperty.Value;

            Assert.True(value.Contains("OAuth2FlowType.AccessCode"));
            Assert.True(value.Contains("OAuth2FlowType.Implicit"));
        }

        [Fact]
        public void CreateServiceAndRequestClassesHasMultipleResponseTypes()
        {
            var serviceNamespace = "SampleApiNamespace.Services";

            var codeGen = new SwaggerToClass("sampleapi.json");

            var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

            var flowProperty = serviceClass.
                Properties
                .FirstOrDefault(p => p.Name == "AllowedResponseTypes");

            Assert.NotNull(flowProperty);

            var value = flowProperty.Value;

            Assert.True(value.Contains("OAuth2ResponseType.Code"));
            Assert.True(value.Contains("OAuth2ResponseType.Token"));
            Assert.True(value.Contains("OAuth2ResponseType.IdTokenToken"));
        }
    }
}

