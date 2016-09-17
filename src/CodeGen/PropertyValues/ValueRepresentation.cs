using System.Collections.Generic;

namespace CodeGen
{
    public abstract class ValueRepresentation
    {
        public abstract List<string> GetNamespaces();
        public abstract string GetPropertyValue(
            bool isAutoProperty,
            bool hasPublicGetter,
            bool hasPublicSetter);
    }
}
