using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using Foundations.Enums;
using Foundations.Extensions;
using Material.Contracts;
using Material.Enums;
using Material.Metadata;

namespace Material.Infrastructure
{
    public abstract class OAuthRequest : Request
    {
        public abstract string Host { get; }
        public abstract string Path { get; }
        public abstract string HttpMethod { get; }
        public abstract List<MediaType> Produces { get; }
        public abstract List<MediaType> Consumes { get; }
        public virtual List<string> RequiredScopes { get; } =
            new List<string>();
        public virtual List<HttpStatusCode> ExpectedStatusCodes { get; } =
            new List<HttpStatusCode> { HttpStatusCode.OK };

        public Dictionary<HttpRequestHeader, string> Headers { get; } =
            new Dictionary<HttpRequestHeader, string>();
        public IDictionary<string, string> QuerystringParameters =>
            GetParameters(RequestParameterType.Query);
        public IDictionary<string, string> PathParameters =>
            GetParameters(RequestParameterType.Path);

        public object Body { get; set; }
        public MediaType BodyType { get; set; } = MediaType.Json;
        public MediaType? OverriddenResponseMediaType { get; set; }

        public virtual void AddUserIdParameter(string userId) { }

        protected virtual IDictionary<string, string> GetParameters(RequestParameterType type)
        {
            var parameterProperties = this.GetPropertiesWhere(prop =>
                prop.GetCustomAttribute<ParameterTypeAttribute>()?.TypeOfParameter == type);

            var dictionary = new Dictionary<string, string>();
            foreach (var parameterProperty in parameterProperties)
            {
                var name = parameterProperty.GetCustomAttribute<NameAttribute>().Value;
                var rawValue = parameterProperty.GetValue(this);

                var format = parameterProperty.GetInterfaceAttribute<IParameterFormatter>();

                if (format == null)
                {
                    throw new NotSupportedException(StringResources.FormatMetadataMissing);
                }

                var value = format.FormatAsString(rawValue);

                if (parameterProperty.GetCustomAttribute<RequiredAttribute>() != null &&
                    value == null)
                {
                    throw new ArgumentNullException(name);
                }

                if (value != null)
                {
                    dictionary.Add(name, value);
                }
            }

            return new ReadOnlyDictionary<string, string>(dictionary);
        }
    }
}

