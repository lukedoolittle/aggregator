using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CodeGen
{
    [DataContract]
    public class SwaggerDefinition
    {
        [DataMember(Name = "swagger")]
        public string SwaggerInfo { get; set; }

        [DataMember(Name = "info")]
        public ApiInfo ApiInfo { get; set; }

        [DataMember(Name = "host")]
        public string Host { get; set; }

        [DataMember(Name = "basePath")]
        public string BasePath { get; set; }

        [DataMember(Name = "schemes")]
        public IList<string> SupportedSchemes { get; set; }

        [DataMember(Name = "produces")]
        public IList<string> ProducesMediaType { get; set; }

        [DataMember(Name = "consumes")]
        public IList<string> ConsumesMediaType { get; set; }

        [DataMember(Name = "paths")]
        public IDictionary<string, PathDefinition> Paths { get; set; }

        [DataMember(Name = "securityDefinitions")]
        public IDictionary<string, SecurityDefinition> SecurityDefinitions { get; set; }

        [DataMember(Name = "definitions")]
        public IList<object> TypeDefinitions { get; set; }
    }

    [DataContract]
    public class ApiInfo
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "termsOfService")]
        public string TermsOfService { get; set; }

        [DataMember(Name = "version")]
        public string Version { get; set; }
    }

    #region Paths

    [DataContract]
    public class Parameter
    {
        [DataMember(Name = "in")]
        public string In { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "required")]
        public bool Required { get; set; }

        [DataMember(Name = "format")]
        public string Format { get; set; }
    }

    [DataContract]
    public class Security
    {
        [DataMember(Name = "oauth2")]
        public IList<string> Oauth2 { get; set; }
    }

    [DataContract]
    public class ResponseDescription
    {
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }

    [DataContract]
    public class PathDefinition
    {
        [DataMember(Name = "summary")]
        public string Summary { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "operationId")]
        public string OperationId { get; set; }

        [DataMember(Name = "parameters")]
        public IList<Parameter> Parameters { get; set; }

        [DataMember(Name = "responses")]
        public IDictionary<string, ResponseDescription> Responses { get; set; }

        [DataMember(Name = "tags")]
        public IList<string> Tags { get; set; }

        [DataMember(Name = "security")]
        public IList<IDictionary<string, IList<string>>> Security { get; set; }
    }

    #endregion Paths

    #region Security

    [DataContract]
    public class SecurityDefinition
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "flow")]
        public string Flow { get; set; }

        [DataMember(Name = "authorizationUrl")]
        public string AuthorizationUrl { get; set; }

        [DataMember(Name = "tokenUrl")]
        public string TokenUrl { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "in")]
        public string ParameterLocation { get; set; }

        [DataMember(Name = "x-grant-types")]
        public IList<string> GrantTypes { get; set; }

        [DataMember(Name = "x-response-types")]
        public IList<string> ResponseTypes { get; set; }

        [DataMember(Name = "x-pkce-support")]
        public bool PkceSupport { get; set; }

        [DataMember(Name = "x-openid-discovery-url")]
        public string OpenIdDiscoveryUrl { get; set; }

        [DataMember(Name = "x-openid-issuers")]
        public IList<string> OpenIdIssuers { get; set; }

        [DataMember(Name = "x-custom-scheme-support")]
        public bool CustomSchemeSupport { get; set; }

        [DataMember(Name = "scopes")]
        public IDictionary<string, string> Scopes { get; set; }
    }

    #endregion Security

}
