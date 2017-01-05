using System.Collections.Generic;
using System.Linq;
using CodeGen;
using Xunit;

namespace Quantfabric.Test.Material.Unit
{
    [Trait("Category", "Continuous")]
    public class CodeGenTests
    {
        private const string SAMPLE_OAUTH2_SWAGGER_DOC = "Material/TestData/sampleoauth2api.json";
        private const string SAMPLE_OAUTH1_SWAGGER_DOC = "Material/TestData/sampleoauth1api.json";
        private const string SAMPLE_API_KEY_SWAGGER_DOC = "Material/TestData/sampleapikeyapi.json";
        private const string SAMPLE_API_KEY_EXCHANGE_SWAGGER_DOC = "Material/TestData/sampleapikeyexchange.json";
        private const string SAMPLE_OPENID_SWAGGER_DOC = "Material/TestData/sampleopenidapi.json";

        [Fact]
        public void DeserializeOAuth1SwaggerFile()
        {
            var codeGen = new SwaggerToClass(SAMPLE_OAUTH1_SWAGGER_DOC);

            var result = codeGen.CreateResourceProvider();

            var boxedProvider = result as BoxedOAuth1ResourceProvider;

            Assert.NotNull(boxedProvider);
            Assert.NotNull(boxedProvider.AuthorizationUrl);
            Assert.NotNull(boxedProvider.TokenUrl);
            Assert.NotNull(boxedProvider.RequestUrl);
            Assert.NotEmpty(boxedProvider.Comments);
            Assert.NotEmpty(boxedProvider.Name);
        }

        [Fact]
        public void DeserializeOAuth2SwaggerFile()
        {
            var codeGen = new SwaggerToClass(SAMPLE_OAUTH2_SWAGGER_DOC);

            var result = codeGen.CreateResourceProvider();

            var boxedProvider = result as BoxedOAuth2ResourceProvider;

            Assert.NotNull(boxedProvider);
            Assert.NotNull(boxedProvider.AuthorizationUrl);
            Assert.NotNull(boxedProvider.TokenUrl);
            Assert.NotEmpty(boxedProvider.Comments);
            Assert.NotEmpty(boxedProvider.Name);
        }

        [Fact]
        public void DeserializeOpenIdSwaggerFile()
        {
            var codeGen = new SwaggerToClass(SAMPLE_OPENID_SWAGGER_DOC);

            var result = codeGen.CreateResourceProvider();

            var boxedProvider = result as BoxedOpenIdResourceProvider;

            Assert.NotNull(boxedProvider);
            Assert.NotNull(boxedProvider.AuthorizationUrl);
            Assert.NotNull(boxedProvider.TokenUrl);
            Assert.NotNull(boxedProvider.OpenIdDiscoveryUrl);
            Assert.NotEmpty(boxedProvider.Comments);
            Assert.NotEmpty(boxedProvider.Name);
        }

        [Fact]
        public void DeserializeApiKeySwaggerFile()
        {
            var codeGen = new SwaggerToClass(SAMPLE_API_KEY_SWAGGER_DOC);

            var result = codeGen.CreateResourceProvider();

            var boxedProvider = result as BoxedApiKeyResourceProvider;

            Assert.NotNull(boxedProvider);
            Assert.NotEmpty(boxedProvider.KeyName);
            Assert.NotNull(boxedProvider.KeyType);
            Assert.NotEmpty(boxedProvider.Comments);
            Assert.NotEmpty(boxedProvider.Name);
        }

        [Fact]
        public void DeserializeApiKeyExchangeSwaggerFile()
        {
            var codeGen = new SwaggerToClass(SAMPLE_API_KEY_EXCHANGE_SWAGGER_DOC);

            var result = codeGen.CreateResourceProvider();

            var boxedProvider = result as BoxedApiKeyExchangeResourceProvider;

            Assert.NotNull(boxedProvider);
            Assert.NotEmpty(boxedProvider.KeyName);
            Assert.NotNull(boxedProvider.KeyType);
            Assert.NotEmpty(boxedProvider.TokenName);
            Assert.NotNull(boxedProvider.TokenUrl);
            Assert.NotEmpty(boxedProvider.Comments);
            Assert.NotEmpty(boxedProvider.Name);
        }



        //[Fact]
        //public void CreateServiceAndRequestClassesCanPrettyPrint()
        //{
        //    var serviceNamespace = "SampleApiNamespace.Services";
        //    var requestNamespace = "SampleApiNamespace.Requests";

        //    var codeGen = new SwaggerToClass(SAMPLE_OAUTH2_SWAGGER_DOC);

        //    var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

        //    var requestClasses = codeGen.GenerateRequestClasses(
        //        requestNamespace,
        //        serviceClass.Name,
        //        serviceNamespace);

        //    var allClasses = new List<ClassRepresentation>
        //    {
        //        serviceClass
        //    };
        //    allClasses.AddRange(requestClasses);

        //    foreach (var myClass in allClasses)
        //    {
        //        Assert.NotNull(myClass.GetNamespaces());
        //        foreach (var metadata in myClass.Metadatas)
        //        {
        //            Assert.NotNull(metadata.TypeName + metadata.ConstructorArguments);
        //        }
        //        foreach (var property in myClass.Properties)
        //        {
        //            Assert.NotNull(property.Metadatas);
        //            Assert.NotNull(property.AccessModifier);
        //            Assert.NotNull(property.Modifier);
        //            Assert.NotNull(property.TypeName);
        //            Assert.NotNull(property.Name);
        //            Assert.NotNull(property.Value);
        //        }
        //        foreach (var myEnum in myClass.Enums)
        //        {
        //            foreach(var item in myEnum.Values)
        //            {
        //                Assert.NotNull(item.Key);
        //                Assert.NotNull(item.Value);
        //            }
        //        }
        //    }
        //}

        //[Fact]
        //public void CreateServiceAndRequestClassesHaveProducesAndConsumes()
        //{
        //    var serviceNamespace = "SampleApiNamespace.Services";
        //    var requestNamespace = "SampleApiNamespace.Requests";

        //    var codeGen = new SwaggerToClass(SAMPLE_OAUTH2_SWAGGER_DOC);

        //    var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

        //    var requestClasses = codeGen.GenerateRequestClasses(
        //        requestNamespace, 
        //        serviceClass.Name, 
        //        serviceNamespace);

        //    foreach (var requestClass in requestClasses)
        //    {
        //        var produces = requestClass
        //            .Properties
        //            .FirstOrDefault(p => p.Name == "Produces");

        //        Assert.NotNull(produces);

        //        var consumes = requestClass
        //            .Properties
        //            .FirstOrDefault(p => p.Name == "Consumes");

        //        Assert.NotNull(consumes);
        //    }
        //}

        //[Fact]
        //public void CreateServiceAndRequestClassesHasResponseCode()
        //{
        //    var serviceNamespace = "SampleApiNamespace.Services";
        //    var requestNamespace = "SampleApiNamespace.Requests";

        //    var codeGen = new SwaggerToClass(SAMPLE_OAUTH2_SWAGGER_DOC);

        //    var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

        //    var requestClasses = codeGen.GenerateRequestClasses(
        //        requestNamespace,
        //        serviceClass.Name,
        //        serviceNamespace);

        //    foreach (var requestClass in requestClasses)
        //    {
        //        var expects = requestClass
        //            .Properties
        //            .FirstOrDefault(p => p.Name == "ExpectedStatusCodes");

        //        Assert.NotNull(expects);
        //    }
        //}

        //[Fact]
        //public void CreateServiceClassHasMultipleFlows()
        //{
        //    var serviceNamespace = "SampleApiNamespace.Services";

        //    var codeGen = new SwaggerToClass(SAMPLE_OAUTH2_SWAGGER_DOC);

        //    var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

        //    var flowProperty = serviceClass.
        //        Properties
        //        .FirstOrDefault(p => p.Name == "AllowedFlows");

        //    Assert.NotNull(flowProperty);

        //    var value = flowProperty.Value;

        //    Assert.True(value.Contains("OAuth2FlowType.AccessCode"));
        //    Assert.True(value.Contains("OAuth2FlowType.Implicit"));
        //}

        //[Fact]
        //public void CreateServiceClassHasMultipleResponseTypes()
        //{
        //    var serviceNamespace = "SampleApiNamespace.Services";

        //    var codeGen = new SwaggerToClass(SAMPLE_OAUTH2_SWAGGER_DOC);

        //    var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

        //    var flowProperty = serviceClass.
        //        Properties
        //        .FirstOrDefault(p => p.Name == "AllowedResponseTypes");

        //    Assert.NotNull(flowProperty);

        //    var value = flowProperty.Value;

        //    Assert.True(value.Contains("OAuth2ResponseType.Code"));
        //    Assert.True(value.Contains("OAuth2ResponseType.Token"));
        //    Assert.True(value.Contains("OAuth2ResponseType.IdTokenToken"));
        //}

        //[Fact]
        //public void CreateServiceClassHasCorrectBaseType()
        //{
        //    var serviceNamespace = "SampleApiNamespace.Services";

        //    var codeGen = new SwaggerToClass(SAMPLE_OAUTH2_SWAGGER_DOC);

        //    var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

        //    Assert.Equal("OpenIdResourceProvider", serviceClass.BaseType.TypeName);
        //}

        //[Fact]
        //public void CreateServiceHasValidValueForPkceSupport()
        //{
        //    var serviceNamespace = "SampleApiNamespace.Services";

        //    var codeGen = new SwaggerToClass(SAMPLE_OAUTH2_SWAGGER_DOC);

        //    var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);
        //    var propertyValue =
        //        serviceClass.Properties.Single(p => p.Name == "SupportsPkce").PropertyValue as
        //            ConcreteValueRepresentation;
        //    Assert.Equal(true, (bool)propertyValue.PropertyValue);
        //}

        //[Fact]
        //public void CreateOAuth2ServiceHasValidValueForCustomUrlSupport()
        //{
        //    var serviceNamespace = "SampleApiNamespace.Services";

        //    var codeGen = new SwaggerToClass(SAMPLE_OAUTH2_SWAGGER_DOC);

        //    var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

        //    var propertyValue =
        //        serviceClass.Properties.Single(p => p.Name == "SupportsCustomUrlScheme").PropertyValue as
        //            ConcreteValueRepresentation;
        //    Assert.Equal(true, (bool)propertyValue.PropertyValue);
        //}

        //[Fact]
        //public void CreateOAuth1ServiceHasValidValueForCustomUrlSupport()
        //{
        //    var serviceNamespace = "SampleApiNamespace.Services";

        //    var codeGen = new SwaggerToClass(SAMPLE_OAUTH1_SWAGGER_DOC);

        //    var serviceClass = codeGen.GenerateServiceClass(serviceNamespace);

        //    var propertyValue =
        //        serviceClass.Properties.Single(p => p.Name == "SupportsCustomUrlScheme").PropertyValue as
        //            ConcreteValueRepresentation;
        //    Assert.Equal(true, (bool)propertyValue.PropertyValue);
        //}
    }
}

