using System;
using System.Collections.Generic;
using System.Globalization;
using Foundations.Extensions;
using Material.Enums;

namespace Material.Infrastructure.Requests
{
    public partial class GoogleGmailMetadata
    {
        public Nullable<DateTime> Before { get; set; }
        public Nullable<DateTime> After { get; set; }

        protected override IDictionary<string, string> GetParameters(RequestParameterType type)
        {
            if (Before != null)
            {
                if (Q == null)
                {
                    Q = string.Empty;
                }

                Q += string.Format(
                    CultureInfo.InvariantCulture, 
                    "before:{0} ", 
                    Before.Value.ToUnixTimeSeconds());
            }

            if (After != null)
            {
                if (Q == null)
                {
                    Q = string.Empty;
                }

                Q += string.Format(
                    CultureInfo.InvariantCulture, 
                    "after:{0} ", 
                    After.Value.ToUnixTimeSeconds());
            }

            return base.GetParameters(type);
        }
    }
}
