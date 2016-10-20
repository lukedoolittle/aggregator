using System;
using System.Collections.Generic;
using System.Reflection;
using Foundations.Reflection;

namespace Foundations.Extensions
{
    public class TypeMapper
    {
        public Dictionary<Type, Type> ParameterToArgumentTypeMapping { get; } =
            new Dictionary<Type, Type>();

        public void MapTypes(
            Type argumentType,
            Type parameterType)
        {
            if (argumentType == null)
            {
                return;
            }
            if (parameterType == null)
            {
                return;
            }

            if (parameterType.IsGenericParameter)
            {
                if (ParameterToArgumentTypeMapping.ContainsKey(parameterType))
                {
                    if (ParameterToArgumentTypeMapping[parameterType] !=
                        argumentType)
                        throw new MethodReflectionException("Argument type not present in list");
                }
                else
                {
                    ParameterToArgumentTypeMapping.Add(
                        parameterType,
                        argumentType);
                }
            }
            else if (parameterType.GetTypeInfo().IsGenericType)
            {
                if (!argumentType.GetTypeInfo().IsGenericType)
                {
                    MapTypes(argumentType.GetTypeInfo().BaseType, parameterType);
                }
                else
                {
                    var concreteTypeArgs = argumentType
                        .GetTypeInfo()
                        .GenericTypeArguments;
                    var genericTypeArgs = parameterType
                        .GetTypeInfo()
                        .GenericTypeArguments;
                    for (var i = 0; i < genericTypeArgs.Length; i++)
                    {
                        MapTypes(concreteTypeArgs[i], genericTypeArgs[i]);
                    }
                }
            }
        }
    }
}
