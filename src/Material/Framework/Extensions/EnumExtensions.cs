using System;
using System.Reflection;
using Material.Framework.Metadata;

namespace Material.Framework.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts an enum to a string defined by metadata
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string EnumToString(this Enum instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var fieldInfo = instance
                .GetType()
                .GetRuntimeField(
                    instance.ToString());

            var attributes =
                (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                    typeof(DescriptionAttribute), 
                    false);

            return attributes.Length > 0 ? 
                attributes[0].Description : 
                instance.ToString();
        }

        /// <summary>
        /// Converts a string to an enum based on the enums metadata
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum to convert to</typeparam>
        /// <param name="instance">The string in question</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public static TEnum StringToEnum<TEnum>(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var enumType = typeof(TEnum);
            var names = Enum.GetNames(enumType);
            foreach (var name in names)
            {
                var @enum = Enum.Parse(enumType, name);
                if (((Enum)@enum).EnumToString().Equals(instance))
                {
                    return (TEnum)@enum;
                }
            }

            throw new ArgumentException(nameof(instance));
        }
    }
}
