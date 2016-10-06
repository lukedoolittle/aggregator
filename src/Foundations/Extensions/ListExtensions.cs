using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundations.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Adds only distinct elements not already in the list
        /// </summary>
        /// <typeparam name="T">The list type</typeparam>
        /// <param name="instance"></param>
        /// <param name="itemsToAdd"></param>
        public static void AddUnique<T>(
            this List<T> instance, 
            List<T> itemsToAdd)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var uniqueItems = itemsToAdd
                .Distinct()
                .Where(i => !instance.Contains(i));

            instance.AddRange(uniqueItems);
        }

        public static void ForEach<T>(
            this IEnumerable<T> instance, 
            Action<T> action)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (action== null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in instance)
            {
                action(item);
            }
        }
    }
}
