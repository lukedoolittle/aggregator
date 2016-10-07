using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Foundations.Extensions;

namespace Foundations.Reflection
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public static class Reflection
    {
        public static Type CreateGenericType(
            Type genericType,
            params Type[] genericParameterTypes)
        {
            if (genericType == null)
            {
                throw new ArgumentNullException(nameof(genericType));
            }

            var type = genericType.MakeGenericType(genericParameterTypes);
            return type;
        }

        #region Types

        public static IEnumerable<Type> GetAllConcreteImplementations(
            Type baseType, 
            Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            return assembly.DefinedTypes
                .Where(type => !type.IsAbstract && type.AsType().HasBase(baseType))
                .Select(t => t.AsType());
        } 

        #endregion Types

        #region Method Invocation

        public static object InvokeGenericMethod(
            object instance,
            string genericMethodName,
            Type[] genericMethodParameters,
            params object[] parameters)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var method = instance.GetType().GetTypeInfo().GetMethodInfo(
                genericMethodName);

            if (method == null)
            {
                
                throw new MethodReflectionException(
                    string.Format(
                        CultureInfo.InvariantCulture, 
                        "No method found named {0}", 
                        genericMethodName));
            }

            if (!method.IsGenericMethod)
            {
                throw new MethodReflectionException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Generic method {0} is not generic",
                        method.Name));
            }

            if (genericMethodParameters == null || genericMethodParameters.Any(p => p == null))
            {
                throw new MethodReflectionException(
                    string.Format(
                        CultureInfo.InvariantCulture, 
                        "Generic method parameters for generic method {0} invalid", 
                        method.Name));
            }

            var generic = method.MakeGenericMethod(genericMethodParameters);

            if (generic == null)
            {
                throw new MethodReflectionException(
                    string.Format(
                        CultureInfo.InvariantCulture, 
                        "Could not create generic type from {0}",
                        method.Name));
            }

            try
            {
                return generic.Invoke(instance, parameters);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        private static MethodInfo GetMethodInfo(
            this TypeInfo instance, 
            string methodName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance.GetDeclaredMethod(methodName) ?? 
                instance.BaseType.GetTypeInfo().GetMethodInfo(methodName);
        }

        public static object InvokeGenericMethod(
            object instance, 
            string genericMethodName,
            Type genericMethodParameter,
            params object[] parameters)
        {
            return InvokeGenericMethod(instance, genericMethodName, new Type[]{ genericMethodParameter}, parameters);
        }

        #endregion MethodInvocation
    }
}
