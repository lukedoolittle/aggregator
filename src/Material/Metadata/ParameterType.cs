using System;
using Material.Enums;

namespace Material.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ParameterTypeAttribute : Attribute
    {
        public RequestParameterType TypeOfParameter { get; }

        public ParameterTypeAttribute(
            RequestParameterType typeOfParameter)
        {
            TypeOfParameter = typeOfParameter;
        }
    }
}
