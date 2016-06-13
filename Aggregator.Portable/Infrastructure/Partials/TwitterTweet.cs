using System;
using System.Collections.Generic;

namespace Aggregator.Infrastructure.Requests
{
    public partial class TwitterTweet : OAuthRequest
    {
        public static string IdParameter => "user_id";
    }
}
