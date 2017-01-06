using System;
using System.Collections.Generic;

namespace CodeGen.Mappings
{
    public class ParameterToTypeMap
    {
        private static readonly Dictionary<Tuple<string, string, bool>, string> _mappings = 
            new Dictionary<Tuple<string, string, bool>, string>
            {
                { new Tuple<string, string, bool>("string", null, true) , typeof(string).Name },
                { new Tuple<string, string, bool>("string", null, false) , typeof(string).Name },
                { new Tuple<string, string, bool>("string", "binary", true) , typeof(int).Name },
                { new Tuple<string, string, bool>("string", "binary", false) , typeof(int?).Name },
                { new Tuple<string, string, bool>("string", "byte", false) , typeof(byte).Name },
                { new Tuple<string, string, bool>("string", "byte", false) , typeof(byte?).Name },
                { new Tuple<string, string, bool>("string", "date", false) , typeof(DateTime?).Name },
                { new Tuple<string, string, bool>("string", "date", true) , typeof(DateTime).Name },
                { new Tuple<string, string, bool>("string", "date-time", false) , typeof(DateTime?).Name },
                { new Tuple<string, string, bool>("string", "date-time", true) , typeof(DateTime).Name },
                { new Tuple<string, string, bool>("string", "date-time-offset", false) , typeof(DateTimeOffset?).Name },
                { new Tuple<string, string, bool>("string", "date-time-offset", true) , typeof(DateTimeOffset).Name },
                { new Tuple<string, string, bool>("string", "uuid", false) , typeof(Guid?).Name },
                { new Tuple<string, string, bool>("string", "uuid", true) , typeof(Guid).Name },
                { new Tuple<string, string, bool>("number", null, false) , typeof(double?).Name },
                { new Tuple<string, string, bool>("number", null, true) , typeof(double).Name },
                { new Tuple<string, string, bool>("number", "double", false) , typeof(double?).Name },
                { new Tuple<string, string, bool>("number", "double", true) , typeof(double).Name },
                { new Tuple<string, string, bool>("number", "float", false) , typeof(float?).Name },
                { new Tuple<string, string, bool>("number", "float", true) , typeof(float).Name },
                { new Tuple<string, string, bool>("integer", null, false) , typeof(int?).Name },
                { new Tuple<string, string, bool>("integer", null, true) , typeof(int).Name },
                { new Tuple<string, string, bool>("integer", "int32", false) , typeof(int?).Name },
                { new Tuple<string, string, bool>("integer", "int32", true) , typeof(int).Name },
                { new Tuple<string, string, bool>("integer", "int64", false) , typeof(long?).Name },
                { new Tuple<string, string, bool>("integer", "int64", true) , typeof(long).Name },
                { new Tuple<string, string, bool>("boolean", null, false) , typeof(bool?).Name },
                { new Tuple<string, string, bool>("boolean", null, true) , typeof(bool).Name },
                { new Tuple<string, string, bool>("array", null, false) , typeof(object[]).Name },
                { new Tuple<string, string, bool>("array", null, true) , typeof(object[]).Name },
            };

        public string Map(Parameter parameter)
        {
            if (parameter.Enumeration != null)
            {
                PrintingFormatter.JsonNameAsCSharpPropertyName(parameter.Name);
            }

            var param = new Tuple<string, string, bool>(
                parameter.Type, 
                parameter.Format, 
                parameter.Required);
            string value = null;

            if (_mappings.TryGetValue(param, out value))
            {
                return value;
            }

            throw new NotSupportedException($"The parameter type {parameter.Type}-{parameter.Format} is not supported");
        }
    }
}
