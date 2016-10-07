using System;

namespace Foundations
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    public class MethodReflectionException : Exception
    {
        public MethodReflectionException(string message) : base(message)
        { }
    }
}
