using System;

namespace Material.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RequiredAttribute : Attribute
    {
    }
}
