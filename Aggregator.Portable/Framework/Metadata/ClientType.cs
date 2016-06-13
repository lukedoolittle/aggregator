using System;

namespace Aggregator.Framework.Metadata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ClientType : Attribute
    {
        public Type Type { get; set; }

        public ClientType(Type type)
        {
            Type = type;
        }
    }
}
