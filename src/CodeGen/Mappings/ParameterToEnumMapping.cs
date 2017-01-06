using System.Linq;

namespace CodeGen.Mappings
{
    public class ParameterToEnumMapping
    {
        public BoxedEnum Map(
            Parameter parameter, 
            string namePrefix)
        {
            if (parameter.Enumeration == null)
            {
                return null;
            }

            return new BoxedEnum(
                namePrefix + parameter.Name,
                parameter.Enumeration.ToDictionary(
                    PrintingFormatter.JsonNameAsCSharpPropertyName, 
                    e => e));
        }
    }
}
