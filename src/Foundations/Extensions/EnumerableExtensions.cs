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

        //TODO: clean this up and comment
        public static string Concatenate(
            this IEnumerable<KeyValuePair<string, string>> collection, 
            string separator, 
            string spacer)
        {
            StringBuilder sb = new StringBuilder();
            int total = collection.Count();
            int count = 0;

            foreach (var item in collection)
            {
                sb.Append(item.Key);
                sb.Append(separator);
                sb.Append(item.Value);

                count++;

                if (count < total)
                {
                    sb.Append(spacer);
                }
            }

            return sb.ToString();
        }

        //TODO: clean this up and comment
        public static string Concatenate(
            this IEnumerable<string> strings,
            string separator)
        {
            StringBuilder sb = new StringBuilder();
            int total = strings.Count();
            int count = 0;

            foreach (var item in strings)
            {
                sb.Append(item);

                count++;

                if (count < total)
                {
                    sb.Append(separator);
                }
            }

            return sb.ToString();
        }
    }
}
