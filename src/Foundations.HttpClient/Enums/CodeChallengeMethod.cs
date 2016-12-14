using Foundations.Attributes;

namespace Foundations.HttpClient.Enums
{
    public enum CodeChallengeMethod
    {
        [Description("plain")] Plain,
        [Description("S256")] Sha256,
    }
}
