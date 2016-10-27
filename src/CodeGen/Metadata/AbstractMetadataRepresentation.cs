using System;
using System.Collections.Generic;

namespace CodeGen
{
    public class AbstractMetadataRepresentation : MetadataRepresentation
    {
        public override string TypeName { get; }
        public override List<string> Namespace { get; }
        public override string ConstructorArguments { get; }

        public AbstractMetadataRepresentation(
            Type type,
            string additionalNamespace,
            string constructorArgument)
        {
            var typeName = type.Name;
            TypeName = typeName.EndsWith("Attribute") ? 
                typeName.Substring(0, typeName.LastIndexOf("Attribute")) :
                typeName;
            
            Namespace = new List<string> { type.Namespace, additionalNamespace };
            ConstructorArguments = "typeof(" + constructorArgument + ")";
        }
    }
}
