using System.Collections.Generic;

namespace CodeGen
{
    public abstract class MetadataRepresentation
    {
        public abstract string TypeName { get; }
        public abstract List<string> Namespace { get; }
        public abstract string ConstructorArguments { get; }
    }
}
