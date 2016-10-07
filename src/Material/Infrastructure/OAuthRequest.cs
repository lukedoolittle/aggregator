using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using Foundations;
using Foundations.Extensions;
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
            GetParameters(RequestParameterTypeEnum.Query);
        public virtual IDictionary<string, string> PathParameters => 
            GetParameters(RequestParameterTypeEnum.Path);

        public object Body { get; set; }
        public MediaType BodyType { get; set; } = MediaType.Json;

        public virtual void AddUserIdParameter(string userId) {}

        protected virtual IDictionary<string, string> GetParameters(RequestParameterTypeEnum type)
        {
            var parameterProperties = this.GetPropertiesWhere(prop =>
                prop.GetCustomAttribute<ParameterType>()?.Type == type);

            var dictionary = new Dictionary<string, string>();
            foreach (var parameterProperty in parameterProperties)
            {
                var format = parameterProperty.GetCustomAttribute<Format>();
                var name = parameterProperty.GetCustomAttribute<Name>().Value;
                var value = ToString(
                    parameterProperty.GetValue(this), 
                    format?.Formatter);

                if (value != null)
                {
                    dictionary.Add(name, value);
                }
            }

            return new ReadOnlyDictionary<string, string>(dictionary);
        }

        //TODO: Format attribute should do its own conversion polymorphically then we can get rid of this
        private static string ToString(
            object instance, 
            string formatter)
        {
            if (instance == null)
            {
                return null;
            }

            if (instance is DateTimeOffset)
            {
                switch (formatter)
                {
                    case "ddd":
                        return ((DateTimeOffset) instance).ToUnixTimeSeconds().ToString();
                    case "d":
                        return ((DateTimeOffset)instance).ToUnixTimeDays().ToString();
                    default:
                        return ((DateTimeOffset)instance).ToString(formatter);
                }
            }
            else if (instance is DateTime)
            {
                switch (formatter)
                {
                    case "ddd":
                        return ((DateTime)instance).ToUnixTimeSeconds().ToString();
                    case "d":
                        return ((DateTime)instance).ToUnixTimeDays().ToString();
                    default:
                        return ((DateTime)instance).ToString(formatter);
                }
            }
            else if (instance.GetType().GetTypeInfo().IsEnum)
            {
                return (instance as Enum).EnumToString();
            }
            else if (instance is bool)
            {
                return instance.ToString().ToLower();
            }
            else
            {
                return instance.ToString();
            }
        }
    }
}

