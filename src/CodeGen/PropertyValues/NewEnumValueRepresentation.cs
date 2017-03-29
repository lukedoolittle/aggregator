using System.Collections.Generic;

namespace CodeGen.PropertyValues
{
    public class NewEnumValueRepresentation : ValueRepresentation
    {
        private readonly string _enumTypeName;
        private readonly string _enumValue;

        public NewEnumValueRepresentation(
            string enumTypeName, 
            string enumValue)
        {
            _enumTypeName = enumTypeName;
            _enumValue = enumValue;
        }

        public override List<string> GetNamespaces()
        {
            return new List<string>();
        }

        public override string GetPropertyValue(
            bool isAutoProperty, 
            bool hasPublicGetter, 
            bool hasPublicSetter)
        {
            if (isAutoProperty)
            {
                return "{ get; set; } = " + _enumTypeName + "." + _enumValue + ";";
            }
            else if (hasPublicGetter)
            {
                return "{ get; } = " + _enumTypeName + "." + _enumValue + ";";
            }
            else if (hasPublicSetter)
            {
                return "{ set; } = " + _enumTypeName + "." + _enumValue + ";";
            }
            else
            {
                return " = " + _enumTypeName + "." + _enumValue + ";";
            }
        }
    }
}
