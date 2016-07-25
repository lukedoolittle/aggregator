using System;
using System.Collections.Generic;
using Foundations.Extensions;
using Material.Enums;

namespace Material.Infrastructure.Requests
{
    public partial class GoogleGmailMetadata
    {
        public Nullable<DateTime> Before { get; set; }
        public Nullable<DateTime> After { get; set; }

        protected override IDictionary<string, string> GetParameters(RequestParameterTypeEnum type)
        {
            if (Before != null)
            {
                if (Q == null)
                {
                    Q = string.Empty;
                }

                Q += $"before:{Before.Value.ToUnixTimeSeconds()} ";
            }

            if (After != null)
            {
                if (Q == null)
                {
                    Q = string.Empty;
                }

                Q += $"after:{After.Value.ToUnixTimeSeconds()} ";
            }

            return base.GetParameters(type);
        }
    }
}
