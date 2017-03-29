namespace Material.HttpClient.Canonicalizers
{
    public interface IHttpRequestCanonicalizer
    {
        string CanonicalizeHttpRequest(HttpRequestBuilder request);
    }
}
