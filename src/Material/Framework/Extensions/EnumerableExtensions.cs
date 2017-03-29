using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Material.Framework.Collections;

namespace Material.Framework.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Create a subset from a range of indices
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] RangeSubset<T>(
            this T[] array,
            int startIndex,
            int length)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            var subset = new T[length];
            Array.Copy(array, startIndex, subset, 0, length);
            return subset;
        }

        /// <summary>
        /// Url encodes each pair in a list
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static HttpValueCollection EncodeParameters(
            this HttpValueCollection instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var newCollection = new HttpValueCollection();

            foreach (var value in instance)
            {
                newCollection.Add(
                    value.Key.UrlEncodeString(), 
                    value.Value.UrlEncodeString());
            }

            return newCollection;
        }

        /// <summary>
        /// Converts a dictionary into a string with given seperator and spacer
        /// </summary>
        /// <param name="instance">Collection to contatenate</param>
        /// <param name="separator">String to seperate each key and value</param>
        /// <param name="spacer">String to seperate each key-value pair</param>
        /// <returns></returns>
        public static string Concatenate(
            this HttpValueCollection instance, 
            string separator, 
            string spacer)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var stringBuilder = new StringBuilder();
            var total = instance.Count();
            var count = 0;

            foreach (var item in instance)
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
        /// <param name="instance">List of string to concatenate</param>
        /// <param name="separator">Seperator to put between each string</param>
        /// <returns></returns>
        public static string Concatenate(
            this IEnumerable<string> instance,
            string separator)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var stringBuilder = new StringBuilder();
            var total = instance.Count();
            var count = 0;

            foreach (var item in instance)
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
