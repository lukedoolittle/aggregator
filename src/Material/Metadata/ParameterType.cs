using System;
using Material.Enums;

namespace Material.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParameterType : Attribute
    {
        public RequestParameterType Type { get; set; }

        public ParameterType(RequestParameterType type)
        {
            Type = type;
        }
    }
}
