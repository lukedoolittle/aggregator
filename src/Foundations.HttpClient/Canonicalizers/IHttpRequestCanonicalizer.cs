namespace Foundations.HttpClient.Canonicalizers
{
    public interface IHttpRequestCanonicalizer
    {
        string CanonicalizeHttpRequest(HttpRequestBuilder request);
    }
}
