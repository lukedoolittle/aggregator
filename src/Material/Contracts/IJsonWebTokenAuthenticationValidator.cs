using Material.Authentication.Validation;
using Material.Domain.Credentials;

namespace Material.Contracts
{
    public interface IJsonWebTokenAuthenticationValidator
    {
        TokenValidationResult IsTokenValid(JsonWebToken token);
    }
}
