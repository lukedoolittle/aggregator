using Material.Infrastructure.Credentials;
using Material.OAuth.Authentication;

namespace Material.Contracts
{
    public interface IJsonWebTokenAuthenticationValidator
    {
        TokenValidationResult IsTokenValid(JsonWebToken token);
    }
}
