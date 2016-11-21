using System;

namespace Foundations.Extensions
{
    // http://stackoverflow.com/questions/1792470/subset-of-array-in-c-sharp
    public static class ArrayExtensions
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

            T[] subset = new T[length];
            Array.Copy(array, startIndex, subset, 0, length);
            return subset;
        }
    }
}
