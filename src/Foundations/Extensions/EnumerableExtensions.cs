using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundations.Collections;

namespace Foundations.Extensions
{
    public static class EnumerableExtensions
    {
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
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

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
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.All(set.Contains);
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
