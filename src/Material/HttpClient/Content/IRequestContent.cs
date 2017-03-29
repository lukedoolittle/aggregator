using System.Net.Http;

namespace Material.HttpClient.Content
{
    public interface IRequestContent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        HttpContent GetContent();
    }
}
