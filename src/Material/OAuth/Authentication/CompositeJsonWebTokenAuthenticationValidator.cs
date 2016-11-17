using System.Collections.Generic;
using System.Linq;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authentication
{
    public class CompositeJsonWebTokenAuthenticationValidator : 
        IJsonWebTokenAuthenticationValidator
    {
        private readonly List<IJsonWebTokenAuthenticationValidator> _validators;

        public CompositeJsonWebTokenAuthenticationValidator(
            params IJsonWebTokenAuthenticationValidator[] validators)
        {
            _validators = validators.ToList();
        }


        public TokenValidationResult IsTokenValid(JsonWebToken token)
        {
            foreach (var validator in _validators)
            {
                var result = validator.IsTokenValid(token);

                if (!result.IsTokenValid)
                {
                    return result;
                }
            }

            return new TokenValidationResult(true);
        }
    }
}
