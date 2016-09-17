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
            TypeName = type.Name;
            Namespace = new List<string> { type.Namespace, additionalNamespace };
            ConstructorArguments = "typeof(" + constructorArgument + ")";
        }
    }
}
