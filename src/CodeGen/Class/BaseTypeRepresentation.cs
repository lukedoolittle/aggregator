using System;

namespace CodeGen
{
    public class BaseTypeRepresentation
    {
        public string TypeName { get; set; }
        public string Namespace { get; set; }

        public BaseTypeRepresentation(string typeName, string @namespace)
        {
            TypeName = typeName;
            Namespace = @namespace;
        }

        public BaseTypeRepresentation(Type type)
        {
            TypeName = type.Name;
            Namespace = type.Namespace;
        }
    }
}
