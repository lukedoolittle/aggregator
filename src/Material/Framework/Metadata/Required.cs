using System;

namespace Material.Framework.Metadata
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RequiredAttribute : Attribute
    {
    }
}
