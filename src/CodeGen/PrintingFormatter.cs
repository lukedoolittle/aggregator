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
    }
}
