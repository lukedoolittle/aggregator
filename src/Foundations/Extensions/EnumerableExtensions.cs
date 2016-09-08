using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundations.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determine if the intersection of this and another list is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="set"></param>
        /// <returns>True if the intersect is empty, false otherwise</returns>
        public static bool IntersectIsEmptySet<T>(
            this IEnumerable<T> instance, 
            IEnumerable<T> set)
        {
            return !instance.All(set.Contains);
        }

        /// <summary>
        /// Determines if an object is a subset of another given collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public static bool IsSubsetOf<T>(
            this IEnumerable<T> instance,
            IEnumerable<T> set)
        {
            return instance.All(set.Contains);
        }

        /// <summary>
        /// Converts a dictionary into a string with given seperator and spacer
        /// </summary>
        /// <param name="collection">Collection to contatenate</param>
        /// <param name="separator">String to seperate each key and value</param>
        /// <param name="spacer">String to seperate each key-value pair</param>
        /// <returns></returns>
        public static string Concatenate(
            this IEnumerable<KeyValuePair<string, string>> collection, 
            string separator, 
            string spacer)
        {
            var stringBuilder = new StringBuilder();
            var total = collection.Count();
            var count = 0;

            foreach (var item in collection)
            {
                stringBuilder.Append(item.Key);
                stringBuilder.Append(separator);
                stringBuilder.Append(item.Value);

                count++;

                if (count < total)
                {
                    stringBuilder.Append(spacer);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Concatenate a list of strings together, joining with a seperator
        /// </summary>
        /// <param name="strings">List of string to concatenate</param>
        /// <param name="separator">Seperator to put between each string</param>
        /// <returns></returns>
        public static string Concatenate(
            this IEnumerable<string> strings,
            string separator)
        {
            var stringBuilder = new StringBuilder();
            var total = strings.Count();
            var count = 0;

            foreach (var item in strings)
            {
                stringBuilder.Append(item);

                count++;

                if (count < total)
                {
                    stringBuilder.Append(separator);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
