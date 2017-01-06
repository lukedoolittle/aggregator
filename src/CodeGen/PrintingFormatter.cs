using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen
{
    public static class PrintingFormatter
    {
        public static string FormatStringList(
            List<string> instance)
        {
            return string.Join(
                ", ", 
                instance.Select(s => $"\"{s}\""));
        }

        public static string FormatEnumList<TEnum>(
            List<TEnum> instance)
        {
            var enumName = typeof(TEnum).Name;

            return string.Join(
                ", ", 
                instance.Select(e => $"{enumName}.{e.ToString()}"));
        }

        public static string JsonNameAsCSharpPropertyName(string jsonName)
        {
            if (string.IsNullOrEmpty(jsonName))
            {
                return string.Empty;
            }

            var result = jsonName.Split(new[] { "_", "-", " " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);

            var upperResult = char.ToUpper(result[0]) + result.Substring(1);

            return upperResult.Replace(".", "");
        }
    }
}
