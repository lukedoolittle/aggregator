using Material.Framework.Enums;

namespace Material.HttpClient.Authenticators
{
    public interface IAuthenticatorParameter
    {
        string Name { get; }
        string Value { get; }
        HttpParameterType Type { get; }
    }
}
