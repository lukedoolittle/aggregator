using System;

namespace CodeGen
{
    public class BoxedPasswordResourceProvider
    {
        public string Name { get; }
        public string Comments { get; }

        public string UsernameKey { get; }
        public string PasswordKey { get; }

        public Uri TokenUrl { get; }

        public BoxedPasswordResourceProvider(
            string name,
            string comments,
            SecurityDefinition security) : 
                this(
                    name, 
                    comments, 
                    security.UsernameKey, 
                    security.PasswordKey, 
                    security.TokenUrl)
        { }

        public BoxedPasswordResourceProvider(
            string name,
            string comments,
            string usernameKey,
            string passwordKey,
            string tokenUrl)
        {
            Name = name;
            Comments = comments;
            UsernameKey = usernameKey;
            PasswordKey = passwordKey;
            TokenUrl = new Uri(tokenUrl);
        }
    }
}
