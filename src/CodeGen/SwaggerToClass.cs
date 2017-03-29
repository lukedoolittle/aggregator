using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using CodeGen.Class;
using CodeGen.Mappings;
using CodeGen.Metadata;
using CodeGen.PropertyValues;
using Material.Domain.Core;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.Framework.Metadata;
using Material.Framework.Metadata.Formatters;
using Newtonsoft.Json.Linq;
using DateTimeFormatterAttribute = Material.Framework.Metadata.Formatters.DateTimeFormatterAttribute;

namespace CodeGen
{
    public class SwaggerToClass
    {
        private readonly SwaggerDefinition _swaggerDefinition;
        private readonly JObject _swagger;

        public SwaggerToClass(string pathToSwaggerFile)
        {
            _swagger = JObject.Parse(File.ReadAllText(pathToSwaggerFile));
            _swaggerDefinition = Deserialize(File.ReadAllText(pathToSwaggerFile));
        }

        public List<BoxedOAuthRequest> CreateRequests()
        {
            var requests = new List<BoxedOAuthRequest>();
            var root = $"{_swaggerDefinition.SupportedSchemes.First()}://{_swaggerDefinition.Host}";
            var basePath = _swaggerDefinition.BasePath;

            foreach (var path in _swaggerDefinition.Paths)
            {
                foreach (var verb in path.Value)
                {
                    var request = verb.Value;

                    var scopes = new List<string>();
                    foreach (var sec in request.Security)
                    {
                        scopes.AddRange(sec["oauth2"]);
                    }

                    var parameters = request.Parameters.Select(parameter => new BoxedProperty(
                            parameter.Description,
                            new ParameterToTypeMap().Map(parameter),
                            PrintingFormatter.JsonNameAsCSharpPropertyName(parameter.Name),
                            new ParameterToValueMapping().Map(parameter),
                            new ParameterToMetadataMapping().Map(parameter),
                            new ParameterToEnumMapping().Map(parameter, request.OperationId)))
                        .ToList();

                    requests.Add(
                        new BoxedOAuthRequest(
                            request.OperationId,
                            request.Description,
                            root,
                            (basePath + path.Key).Replace("//", "/"),
                            verb.Key.ToUpper(),
                            _swaggerDefinition.ProducesMediaType.ToList(),
                            _swaggerDefinition.ConsumesMediaType.ToList(),
                            scopes,
                            request.Responses.Keys.Select(a => Convert.ToInt32(a)).ToList(),
                            parameters));
                }
            }

            return requests;
        }

        public object CreateResourceProvider()
        {
            if (_swaggerDefinition.SecurityDefinitions == null)
            {
                return null;
            }

            var name = _swaggerDefinition.ApiInfo.Title;
            var comments = $"{_swaggerDefinition.ApiInfo.Description} {_swaggerDefinition.ApiInfo.Version}";

            object definition = null;

            foreach (var securityDefinition in _swaggerDefinition.SecurityDefinitions)
            {
                var security = securityDefinition.Value;

                if (security.Type == "oauth2")
                {
                    if (security.Scopes.Keys.Contains("openid"))
                    {
                        var newDefinition = new BoxedOpenIdResourceProvider(
                            name,
                            comments,
                            security);

                        if (definition is BoxedOAuth2ResourceProvider)
                        {
                            newDefinition.Merge(definition as BoxedOAuth2ResourceProvider);
                        }
                        else if (definition is BoxedOpenIdResourceProvider)
                        {
                            newDefinition.Merge(definition as BoxedOpenIdResourceProvider);
                        }

                        definition = newDefinition;
                    }
                    else
                    {
                        var newDefinition = new BoxedOAuth2ResourceProvider(
                            name,
                            comments,
                            security);

                        if (definition is BoxedOAuth2ResourceProvider)
                        {
                            newDefinition.Merge(definition as BoxedOAuth2ResourceProvider);
                        }
                        else if (definition is BoxedOpenIdResourceProvider)
                        {
                            newDefinition.Merge(definition as BoxedOpenIdResourceProvider);
                        }

                        definition = newDefinition;
                    }
                }
                else if (security.Type == "oauth1")
                {
                    definition = new BoxedOAuth1ResourceProvider(
                            name,
                            comments,
                            security);
                }
                else if (security.Type == "apiKey")
                {
                    definition = new BoxedApiKeyResourceProvider(
                            name,
                            comments,
                            security);
                }
                else if (security.Type == "keyJwtExchange")
                {
                    definition = new BoxedApiKeyExchangeResourceProvider(
                            name,
                            comments,
                            security);
                }
                else if (security.Type == "password")
                {
                    definition = new BoxedPasswordResourceProvider(
                        name, 
                        comments, 
                        security);
                }
                else
                {
                    throw new NotSupportedException($"Security type {security.Type} is not supported");
                }
            }

            return definition;
        }

        public SwaggerDefinition Deserialize(string entity)
        {
            var settings = new DataContractJsonSerializerSettings
            {
                UseSimpleDictionaryFormat = true
            };
            var serializer = new DataContractJsonSerializer(
                typeof(SwaggerDefinition),
                settings);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(entity)))
            {
                return (SwaggerDefinition)serializer.ReadObject(stream);
            }
        }
        
        public List<ClassRepresentation> GenerateRequestClasses(
            string @namespace,
            string serviceTypeName,
            string serviceTypeNamespace)
        {
            var classes = new List<ClassRepresentation>();

            var scheme = _swagger["schemes"][0].ToString();
            var domain = _swagger["host"].ToString();
            var pathRoot = _swagger["basePath"].ToString();

            var produces = _swagger["produces"]
                .Select(item => EnumExtensions.StringToEnum<MediaType>(item.ToString()))
                .ToList();
            var consumes = _swagger["consumes"]
                .Select(item => EnumExtensions.StringToEnum<MediaType>(item.ToString()))
                .ToList();

            var paths = _swagger["paths"];

            foreach (JProperty path in paths)
            {
                var pathResidual = path.Name;

                foreach (JProperty request in path.Value)
                {
                    var details = request.Value.ToObject<JObject>();

                    var @class = new ClassRepresentation(
                        details["operationId"].ToString(),
                        @namespace)
                    {
                        Comments = details["summary"].ToString()
                    };

                    @class.Metadatas.Add(new AbstractMetadataRepresentation(
                        typeof(ServiceTypeAttribute), 
                        serviceTypeNamespace, 
                        serviceTypeName));

                    @class.BaseType = new BaseTypeRepresentation(typeof(OAuthRequest));

                    @class.Properties.Add(new PropertyRepresentation(typeof(string), "Host")
                    {
                        IsOverride = true,
                        PropertyValue = new ConcreteValueRepresentation($"{scheme}://{domain}")
                    });
                    @class.Properties.Add(new PropertyRepresentation(typeof(string), "Path")
                    {
                        IsOverride = true,
                        PropertyValue = new ConcreteValueRepresentation((pathRoot + pathResidual).Replace("//", "/"))
                    });
                    @class.Properties.Add(new PropertyRepresentation(typeof(string), "HttpMethod")
                    {
                        IsOverride = true,
                        PropertyValue = new ConcreteValueRepresentation(request.Name.ToUpper())
                    });

                    var requestProduces = produces;

                    if (details["produces"] != null)
                    {
                        requestProduces = details["produces"]
                            .ToObject<JArray>()
                            .Select(t => EnumExtensions.StringToEnum<MediaType>(t.ToString()))
                            .ToList();
                    }

                    @class.Properties.Add(new PropertyRepresentation(typeof(List<MediaType>), "Produces")
                    {
                        IsOverride = true,
                        PropertyValue = new ConcreteValueRepresentation(requestProduces)
                    });

                    var requestConsumes = consumes;

                    if (details["consumes"] != null)
                    {
                        requestConsumes = details["consumes"]
                            .ToObject<JArray>()
                            .Select(t => EnumExtensions.StringToEnum<MediaType>(t.ToString()))
                            .ToList();
                    }

                    @class.Properties.Add(new PropertyRepresentation(typeof(List<MediaType>), "Consumes")
                    {
                        IsOverride = true,
                        PropertyValue = new ConcreteValueRepresentation(requestConsumes)
                    });

                    if (details["responses"] != null)
                    {
                        var acceptableResponseCodes = new List<HttpStatusCode>();
                        foreach (var response in details["responses"])
                        {
                            var statusCode = response.ToObject<JProperty>().Name;
                            acceptableResponseCodes.Add((HttpStatusCode) Convert.ToInt32(statusCode));
                        }

                        @class.Properties.Add(new PropertyRepresentation(typeof(List<HttpStatusCode>), "ExpectedStatusCodes")
                        {
                            IsOverride = true,
                            PropertyValue = new ConcreteValueRepresentation(acceptableResponseCodes)
                        });
                    }

                    if (details["x-request-filter-property"] != null)
                    {
                        @class.Properties.Add(new PropertyRepresentation(typeof(string), "RequestFilterKey")
                        {
                            IsOverride = true,
                            PropertyValue = new ConcreteValueRepresentation(details["x-request-filter-property"].ToString())
                        });
                    }

                    //this is pretty ridgid and makes assumptions about the structure that may not be true
                    if (details["security"]?.ToObject<JArray>()?.Count > 0 &&
                        details["security"]?.ToObject<JArray>()[0]?.ToObject<JObject>()?["oauth2"] != null)
                    {
                        var scopes =
                            details["security"]
                            .ToObject<JArray>()[0]
                            .ToObject<JObject>()["oauth2"]
                            .ToObject<JArray>()
                            .Select(s => s.ToString())
                            .ToList();

                        @class.Properties.Add(new PropertyRepresentation(typeof(List<string>), "RequiredScopes")
                        {
                            IsOverride = true,
                            PropertyValue = new ConcreteValueRepresentation(scopes)
                        });
                    }

                    string requestFilterProperty = null;

                    if (details["parameters"] != null)
                    {
                        foreach (JObject parameter in details["parameters"])
                        {
                            if (parameter["x-request-filter-property"] != null)
                            {
                                requestFilterProperty = parameter["x-request-filter-property"].ToString();
                            }

                            var name = CreateCSharpPropertyName(parameter["name"].ToString());

                            PropertyRepresentation property = null;

                            if (parameter["enum"] != null)
                            {
                                var enumName = @class.Name + name;
                                var enumNamespace = typeof(DescriptionAttribute).Namespace;
                                var @enum = new EnumRepresentation(
                                    enumName, 
                                    enumNamespace);

                                foreach (var item in parameter["enum"])
                                {
                                    @enum.Values.Add(
                                        CreateCSharpPropertyName(item.ToString()), 
                                        item.ToString());
                                }

                                @class.Enums.Add(@enum);

                                ValueRepresentation propertyValue = null;
                                if (parameter["default"] != null)
                                {
                                    propertyValue = new NewEnumValueRepresentation(
                                        @enum.Name,
                                        CreateCSharpPropertyName(parameter["default"].ToString()));
                                }

                                property = new PropertyRepresentation(
                                    @enum.Name,
                                    @namespace,
                                    name)
                                {
                                    IsAutoProperty = true,
                                    PropertyValue = propertyValue,
                                    Comments = parameter["description"].ToString()
                                };
                            }
                            else
                            {
                                var type = GetTypeFromParameterType(
                                    parameter["type"]?.ToString(),
                                    parameter["format"]?.ToString(),
                                    parameter["required"]?.ToString());


                                ConcreteValueRepresentation propertyValue = null;

                                if (parameter["default"] != null)
                                {
                                    propertyValue = new ConcreteValueRepresentation(ConvertType(
                                        parameter["default"].ToString(),
                                        type));
                                }

                                property = new PropertyRepresentation(type, name)
                                {
                                    IsAutoProperty = true,
                                    PropertyValue = propertyValue,
                                    Comments = parameter["description"].ToString()
                                };
                            }

                            var nameMetadata = new ConcreteMetadataRepresentation(typeof(NameAttribute));
                            nameMetadata.ConstructorParameters = new List<object> { parameter["name"].ToString()};
                            property.Metadatas.Add(nameMetadata);


                            if (parameter["in"].ToString() == "query")
                            {
                                var propertyTypeMetadata = new ConcreteMetadataRepresentation(typeof(ParameterTypeAttribute));
                                propertyTypeMetadata.ConstructorParameters = new List<object> { RequestParameterType.Query };
                                property.Metadatas.Add(propertyTypeMetadata);

                            }
                            else if (parameter["in"].ToString() == "path")
                            {
                                var propertyTypeMetadata = new ConcreteMetadataRepresentation(typeof(ParameterTypeAttribute));
                                propertyTypeMetadata.ConstructorParameters = new List<object> { RequestParameterType.Path };
                                property.Metadatas.Add(propertyTypeMetadata);
                            }
                            else if (parameter["in"].ToString() == "header")
                            {
                                var propertyTypeMetadata = new ConcreteMetadataRepresentation(typeof(ParameterTypeAttribute));
                                propertyTypeMetadata.ConstructorParameters = new List<object> { RequestParameterType.Header };
                                property.Metadatas.Add(propertyTypeMetadata);
                            }
                            else
                            {
                                //cover "body" here
                                throw new NotImplementedException();
                            }

                            if (parameter["required"]?.ToString().ToLower() == "true")
                            {
                                property.Metadatas.Add(new ConcreteMetadataRepresentation(typeof(RequiredAttribute)));
                            }

                            var formatMetadata = GetParameterMetadata(
                                parameter["type"]?.ToString(),
                                parameter["format"]?.ToString(),
                                parameter["pattern"]?.ToString(),
                                parameter["enum"] != null);
                            property.Metadatas.Add(formatMetadata);

                            @class.Properties.Add(property);
                        }
                    }


                    //handle the response
                    //if the response contains x-timeseries-information then append the ITimeseries interface
                    //if the response contains the x-response-filter property AND there is a parameter with the x-request-filter property, then implement the IFilterable (and implicitly the ITimeseries) interface

                    classes.Add(@class);
                }
            }

            return classes;
        }

        private ConcreteMetadataRepresentation GetParameterMetadata(
            string type,
            string format, 
            string pattern,
            bool isEnum)
        {
            if (isEnum)
            {
                return new ConcreteMetadataRepresentation(typeof(EnumFormatterAttribute));
            }
            else if (pattern == null)
            {
                return new ConcreteMetadataRepresentation(typeof(DefaultFormatterAttribute));
            }
            else if (format == "byte" ||
                     format == "binary" ||
                     format == "uuid" ||
                     format == "double" ||
                     format == "float" ||
                     format == "int32" ||
                     format == "int64")
            {
                return new ConcreteMetadataRepresentation(typeof(DefaultFormatterAttribute));
            }
            else if (type == "boolean")
            {
                return new ConcreteMetadataRepresentation(typeof(BooleanFormatterAttribute));
            }
            else if (pattern == "ddd")
            {
                if (format == "date-time-offset")
                {
                    return new ConcreteMetadataRepresentation(typeof(UnixTimeSecondsDateTimeOffsetFormatterAttribute));
                }
                else
                {
                    return new ConcreteMetadataRepresentation(typeof(UnixTimeSecondsDateTimeFormatterAttribute));
                }
            }
            else if (pattern == "d")
            {
                if (format == "date-time-offset")
                {
                    return new ConcreteMetadataRepresentation(typeof(UnixTimeDaysDateTimeOffsetFormatterAttribute));
                }
                else
                {
                    return new ConcreteMetadataRepresentation(typeof(UnixTimeDaysDateTimeFormatterAttribute));
                }
            }
            else if (format == "date" || format == "date-time" || format == "date-time-offset")
            {
                if (format == "date-time-offset")
                {
                    return new ConcreteMetadataRepresentation(typeof(DateTimeOffsetFormatterAttribute))
                    {
                        ConstructorParameters = new List<object> {pattern}
                    };
                }
                else
                {
                    return new ConcreteMetadataRepresentation(typeof(DateTimeFormatterAttribute))
                    {
                        ConstructorParameters = new List<object> { pattern }
                    };
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private object ConvertType(
            string item, 
            Type type)
        {
            if (type == typeof(string))
            {
                return item;
            }
            else if (type == typeof(int) || type == typeof(Nullable<int>))
            {
                return Convert.ToInt32(item);
            }
            else if (type == typeof(long) || type == typeof(Nullable<long>))
            {
                return Convert.ToInt64(item);
            }
            else if (type == typeof(DateTime) || type == typeof(Nullable<DateTime>))
            {
                DateTime result;

                if (DateTime.TryParse(item, out result))
                {
                    return result;
                }
                else
                {
                    throw new Exception("Couldn't parse datetime string");
                }
            }
            else if (type == typeof(DateTimeOffset) || type == typeof(Nullable<DateTimeOffset>))
            {
                DateTimeOffset result;

                if (DateTimeOffset.TryParse(item, out result))
                {
                    return result;
                }
                else
                {
                    throw new Exception("Couldn't parse datetime string");
                }
            }
            else if (type == typeof(bool?) || type == typeof(bool))
            {
                if (item.ToLower() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (type == typeof(Guid) || type == typeof(Guid?))
            {
                return Guid.Parse(item);
            }
            else
            {
                throw new Exception("Unhandled object type: " + type.Name);
            }
        }

        private Type GetTypeFromParameterType(
            string type, 
            string format,
            string required)
        {
            var isRequired = required == "true";

            if (type == "string")
            {
                if (format == null)
                {
                    return typeof(string);
                }
                if (format == "binary")
                {
                    return isRequired ? typeof(int) : typeof(int?);
                }
                if (format == "byte")
                {
                    return isRequired ? typeof(byte) : typeof(byte?);
                }
                if (format == "date" || format == "date-time")
                {
                    return isRequired ? typeof(DateTime) : typeof(DateTime?);
                }
                if (format == "date-time-offset")
                {
                    return isRequired ? typeof(DateTimeOffset) : typeof(DateTimeOffset?);
                }
                if (format == "uuid")
                {
                    return isRequired ? typeof(Guid) : typeof(Guid?);
                }
            }
            else if (type == "number")
            {
                if (format == null || format == "double")
                {
                    return isRequired ? typeof(double) : typeof(double?);
                }
                if (format == "float")
                {
                    return isRequired ? typeof(float) : typeof(float?);
                }
            }
            else if (type == "integer")
            {
                if (format == null || format == "int32")
                {
                    return isRequired ? typeof(int) : typeof(int?);
                }
                if (format == "int64")
                {
                    return isRequired ? typeof(long) : typeof(long?);
                }
            }
            else if (type == "array")
            {
                return typeof(object[]);
            }
            else if (type == "boolean")
            {
                return isRequired ? typeof(bool) : typeof(bool?);
            }
            else if (type == "file")
            {
                throw new Exception();
            }

            throw new Exception();
        }

        private string CreateCSharpPropertyName(string jsonName)
        {
            if (string.IsNullOrEmpty(jsonName))
            {
                return string.Empty;
            }

            var result = jsonName.Split(new[] { "_", "-", " ", "$" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);

            var upperResult = char.ToUpper(result[0]) + result.Substring(1);

            return upperResult.Replace(".", "");
        }
    }
}
