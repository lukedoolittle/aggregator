using System.Collections.Generic;
using System.Linq;
using System.Net;
using Foundations.Enums;
using Foundations.Extensions;

namespace CodeGen
{
    public class BoxedOAuthRequest
    {
        public string Name { get; }
        public string Comments { get; }
        public string Host { get; }
        public string Path { get; }
        public string HttpMethod { get; }
        public List<MediaType> Produces { get; }
        public List<MediaType> Consumes { get; }
        public List<string> RequiredScopes { get; }
        public List<HttpStatusCode> ExpectedStatusCodes { get; }
        public List<BoxedProperty> Properties { get; }
        public List<BoxedEnum> Enums { get; }

        public string FormattedProduces => PrintingFormatter.FormatEnumList(Produces);
        public string FormattedConsumes => PrintingFormatter.FormatEnumList(Consumes);
        public string FormattedRequiredScopes => PrintingFormatter.FormatStringList(RequiredScopes);
        public string FormattedExpectedStatusCodes => PrintingFormatter.FormatEnumList(ExpectedStatusCodes);

        public BoxedOAuthRequest(
            string name,
            string comments,
            string host,
            string path,
            string httpMethod,
            List<string> produces,
            List<string> consumes,
            List<string> requiredScopes,
            List<int> expectedStatusCodes,
            List<BoxedProperty> properties)
        {
            Name = name;
            Comments = comments;
            Host = host;
            Path = path;
            HttpMethod = httpMethod;
            Produces = produces.Select(p => p.StringToEnum<MediaType>()).ToList();
            Consumes = consumes.Select(c => c.StringToEnum<MediaType>()).ToList();
            RequiredScopes = requiredScopes;
            ExpectedStatusCodes = expectedStatusCodes.Select(e => (HttpStatusCode)e).ToList();
            Properties = properties;
            Enums = properties
                .Where(p => p.AssociatedEnum != null)
                .Select(p => p.AssociatedEnum)
                .ToList();
        }
    }
}
