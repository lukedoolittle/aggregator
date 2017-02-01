using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Foundations.Extensions;

namespace Material.Infrastructure.Credentials
{
    public class PasswordCredentials : TokenCredentials
    {
        public override string ExpiresIn => Cookies
            ?.Select(c => c.Expires)
            .Min()
            .ToUnixTimeSeconds()
            .ToString(CultureInfo.InvariantCulture);
        public override bool AreValidIntermediateCredentials => Cookies != null;
        public IEnumerable<Cookie> Cookies { get; private set; }

        public PasswordCredentials SetCookies(params Cookie[] cookies)
        {
            Cookies = cookies;
            DateCreated = DateTimeOffset.Now;

            return this;
        }
    }
}
