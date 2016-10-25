using System;

namespace Material.Metadata
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceTypeAttribute : Attribute
    {
        public Type TypeOfService { get; }

        public ServiceTypeAttribute(Type typeOfService)
        {
            TypeOfService = typeOfService;
        }
    }
}
