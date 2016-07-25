using System;
using Material.Enums;

namespace Material.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParameterType : Attribute
    {
        public RequestParameterTypeEnum Type { get; set; }

        public ParameterType(RequestParameterTypeEnum type)
        {
            Type = type;
        }
    }
}
