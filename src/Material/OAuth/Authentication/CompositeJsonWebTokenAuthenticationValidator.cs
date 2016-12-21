using System.Collections.Generic;
using System.Security;
using Material.Contracts;
using Material.Infrastructure.Credentials;

namespace Material.OAuth.Authentication
{
    public class CompositeJsonWebTokenAuthenticationValidator : 
        IJsonWebTokenAuthenticationValidator
    {
        private bool _throwIfInvalid;
        private readonly List<IJsonWebTokenAuthenticationValidator> _validators = 
            new List<IJsonWebTokenAuthenticationValidator>();

        public CompositeJsonWebTokenAuthenticationValidator AddValidator(
            IJsonWebTokenAuthenticationValidator validator)
        {
            _validators.Add(validator);

            return this;
        }

        public CompositeJsonWebTokenAuthenticationValidator ThrowIfInvalid()
        {
            _throwIfInvalid = true;

            return this;
        }

        public TokenValidationResult IsTokenValid(JsonWebToken token)
        {
            foreach (var validator in _validators)
            {
                var result = validator.IsTokenValid(token);

                if (!result.IsTokenValid)
                {
                    if (_throwIfInvalid)
                    {
                        throw new SecurityException(result.Reason);
                    }
                    else
                    {
                        return result;
                    }
                }
            }

            return new TokenValidationResult(true);
        }
    }
}
