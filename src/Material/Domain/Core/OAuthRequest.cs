using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using Material.Contracts;
using Material.Framework.Enums;
using Material.Framework.Extensions;
using Material.Framework.Metadata;
using Material.HttpClient.Content;

namespace Material.Domain.Core
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

        public IDictionary<string, string> Headers =>
            GetParameters(RequestParameterType.Header);
        public IDictionary<string, string> QuerystringParameters =>
            GetParameters(RequestParameterType.Query);
        public IDictionary<string, string> PathParameters =>
            GetParameters(RequestParameterType.Path);

        public MediaType DefaultContentType { get; set; } = MediaType.Json;

        public IList<BodyContent> Content { get; } = 
            new List<BodyContent>();

        public void AddContent(object body)
        {
            Content.Add(new BodyContent(
                body, 
                DefaultContentType, 
                Encoding.UTF8));
        }

        public void AddContent(
            object body, 
            MediaType bodyType)
        {
            Content.Add(new BodyContent(
                body,
                bodyType,
                Encoding.UTF8));
        }

        public MediaType? OverriddenResponseMediaType { get; set; }

        public virtual void AddUserIdParameter(string userId) { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public virtual string GetModifiedHost()
        {
            return Host;
        }

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
                    throw new ArgumentNullException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            StringResources.MissingRequiredRequestParameter,
                            name,
                            Host + Path));
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

