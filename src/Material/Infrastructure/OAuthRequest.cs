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

        public virtual List<string> RequiredScopes { get; } = 
            new List<string>();

        public virtual Dictionary<HttpRequestHeader, string> Headers { get; } = 
            new Dictionary<HttpRequestHeader, string>();

        public virtual IDictionary<string, string> QuerystringParameters => 
            GetParameters(RequestParameterType.Query);
        public virtual IDictionary<string, string> PathParameters => 
            GetParameters(RequestParameterType.Path);

        public object Body { get; set; }
        public MediaType BodyType { get; set; } = MediaType.Json;

        public virtual void AddUserIdParameter(string userId) {}

        protected virtual IDictionary<string, string> GetParameters(RequestParameterType type)
        {
            var parameterProperties = this.GetPropertiesWhere(prop =>
                prop.GetCustomAttribute<ParameterTypeAttribute>()?.TypeOfParameter == type);

            var dictionary = new Dictionary<string, string>();
            foreach (var parameterProperty in parameterProperties)
            {
                var format = parameterProperty.GetInterfaceAttribute<IParameterFormatter>();

                if (format == null)
                {
                    throw new NotSupportedException(StringResources.FormatMetadataMissing);
                }

                var value = format.FormatAsString(parameterProperty.GetValue(this));

                var name = parameterProperty.GetCustomAttribute<NameAttribute>().Value;
                if (value != null)
                {
                    dictionary.Add(name, value);
                }
            }

            return new ReadOnlyDictionary<string, string>(dictionary);
        }
    }
}

