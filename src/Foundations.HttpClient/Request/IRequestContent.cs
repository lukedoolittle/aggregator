using System.Net.Http;

namespace Foundations.HttpClient.Request
{
    public interface IRequestContent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        HttpContent GetContent();
    }
}
