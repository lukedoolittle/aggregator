using System.Collections.Generic;

namespace CodeGen.Mappings
{
    public class ParameterToMetadataMapping
    {
        public List<BoxedMetadata> Map(Parameter parameter)
        {
            var result = new List<BoxedMetadata>();

            if (parameter.Required)
            {
                result.Add(new BoxedMetadata(
                    "RequiredAttribute", 
                    null));
            }

            result.Add(new BoxedMetadata(
                "ParameterTypeAttribute", 
                $"RequestParameterType.{parameter.In}"));

            return result;

            //if (isEnum)
            //{
            //    return new ConcreteMetadataRepresentation(typeof(EnumFormatterAttribute));
            //}
            //else if (pattern == null)
            //{
            //    return new ConcreteMetadataRepresentation(typeof(DefaultFormatterAttribute));
            //}
            //else if (format == "byte" ||
            //         format == "binary" ||
            //         format == "uuid" ||
            //         format == "double" ||
            //         format == "float" ||
            //         format == "int32" ||
            //         format == "int64")
            //{
            //    return new ConcreteMetadataRepresentation(typeof(DefaultFormatterAttribute));
            //}
            //else if (type == "boolean")
            //{
            //    return new ConcreteMetadataRepresentation(typeof(BooleanFormatterAttribute));
            //}
            //else if (pattern == "ddd")
            //{
            //    if (format == "date-time-offset")
            //    {
            //        return new ConcreteMetadataRepresentation(typeof(UnixTimeSecondsDateTimeOffsetFormatterAttribute));
            //    }
            //    else
            //    {
            //        return new ConcreteMetadataRepresentation(typeof(UnixTimeSecondsDateTimeFormatterAttribute));
            //    }
            //}
            //else if (pattern == "d")
            //{
            //    if (format == "date-time-offset")
            //    {
            //        return new ConcreteMetadataRepresentation(typeof(UnixTimeDaysDateTimeOffsetFormatterAttribute));
            //    }
            //    else
            //    {
            //        return new ConcreteMetadataRepresentation(typeof(UnixTimeDaysDateTimeFormatterAttribute));
            //    }
            //}
            //else if (format == "date" || format == "date-time" || format == "date-time-offset")
            //{
            //    if (format == "date-time-offset")
            //    {
            //        return new ConcreteMetadataRepresentation(typeof(DateTimeOffsetFormatterAttribute))
            //        {
            //            ConstructorParameters = new List<object> { pattern }
            //        };
            //    }
            //    else
            //    {
            //        return new ConcreteMetadataRepresentation(typeof(DateTimeFormatterAttribute))
            //        {
            //            ConstructorParameters = new List<object> { pattern }
            //        };
            //    }
            //}
            //else
            //{
            //    throw new NotImplementedException();
            //}
        }
    }
}
