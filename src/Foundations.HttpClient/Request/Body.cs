using System.Text;
using Foundations.Enums;

namespace Foundations.HttpClient.Request
{
    public class Body
    {
        public object Content { get; set; }
        public MediaType MediaType { get; set; }
        public Encoding Encoding { get; set; }
    }
}
