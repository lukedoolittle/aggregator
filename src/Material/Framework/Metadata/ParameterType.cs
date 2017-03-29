using System;
using Material.Framework.Enums;

namespace Material.Framework.Metadata
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
