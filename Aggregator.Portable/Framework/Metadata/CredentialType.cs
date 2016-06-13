using System;

namespace Aggregator.Framework.Metadata
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
