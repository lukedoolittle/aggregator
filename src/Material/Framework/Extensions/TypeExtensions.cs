using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Material.Framework.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetPropertiesWhere(
            this object instance, 
            Func<PropertyInfo, bool> predicate)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance
                .GetType()
                .GetTypeInfo()
                .DeclaredProperties
                .Where(predicate);
        }

        /// <summary>
        /// Determines if the current instance has a particular base type
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static bool HasBase(this Type instance, Type baseType)
        {
            if (baseType == null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }

            if (instance == null)
            {
                return false;
            }

            return instance.GetTypeInfo().BaseType == baseType ||
                   instance.GetTypeInfo().BaseType.HasBase(baseType);
        }

        public static T GetInterfaceAttribute<T>(this MemberInfo instance)
            where T : class
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var attribute = instance
                .GetCustomAttributes()
                .FirstOrDefault(a => a
                    .GetType()
                    .GetTypeInfo()
                    .ImplementedInterfaces
                    .Contains(typeof(T)));

            return attribute as T;
        }

        /// <summary>
        /// Gets all the attributes for a type of the given type T
        /// </summary>
        /// <typeparam name="T">The type of attribute to get</typeparam>
        /// <param name="instance">The type of the object</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this Type instance)
            where T : class
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var info = instance.GetTypeInfo();
            var attributes = info.GetCustomAttributes(true);

            return attributes.OfType<T>().ToList();
        }
    }
}
