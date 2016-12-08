using System;
using Foundation;

namespace Material.View.WebAuthorization
{
    public static class UriExtensions
    {
        public static NSUrl ToNSUrl(this Uri instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            //TODO: determine why spaces are not properly Url encoded
            return new NSUrl(instance.ToString().Replace(" ", "%20"));
        }
    }
}