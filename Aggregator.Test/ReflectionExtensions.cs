using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;

namespace Aggregator.Test.Helpers
{
    public static class ReflectionExtensions
    {
        public static T GetMemberValue<T>(this object instance, string memberName)
        {
            try
            {
                var bindingFlags =
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Static;
                
                var member = instance.GetType().GetField(memberName, bindingFlags);

                if (member != null)
                {
                    return (T) member.GetValue(instance);
                }
            }
            catch
            {
                
            }

            return default(T);
        }

        public static T GetMemberValueForStatic<T>(this Type instance, string memberName)
        {
            var member = instance.GetField(
                memberName,
                BindingFlags.NonPublic |
                BindingFlags.Static);

            return (T)member.GetValue(null);
        }

        public static T GetPropertyValue<T>(this object instance, string memberName)
        {
            try
            {
                var bindingFlags =
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Static;
                var member = instance.GetType().GetProperty(memberName, bindingFlags);

                if (member != null)
                {
                    return (T)member.GetValue(instance);
                }
            }
            catch
            {

            }

            return default(T);
        }

        public static void SetValue(
            this object instance, 
            string name, 
            object value)
        {
            var bindingFlags =
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Static;
            
            var member = instance.GetType().GetField(
                name,
                bindingFlags);

            if (member == null)
            {
                var property = instance.GetType().GetProperty(
                    name,
                    bindingFlags);
                property.SetValue(instance, value);
            }
            else
            {
                member.SetValue(instance, value);
            }
        }

        public static object InvokeGenericExtensionMethod(
            Type extensionType, 
            string genericMethodName,
            Type genericMethodParameters,
            params object[] parameters)
        {
            MethodInfo method = extensionType.GetMethod(
                genericMethodName,
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            MethodInfo generic = method.MakeGenericMethod(genericMethodParameters);
            return generic.Invoke(null, parameters);
        }
    }
}
