using System;

namespace Material.Metadata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CredentialType : Attribute
    {
        public Type Type { get; set; }

        public CredentialType(Type type)
        {
            Type = type;
        }
    }
}
