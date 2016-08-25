using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Foundations.HttpClient.Enums;

namespace CodeGen
{
    public class ConcreteValueRepresentation : ValueRepresentation
    {
        public object PropertyValue { get; }

        public ConcreteValueRepresentation(object propertyValue)
        {
            if (propertyValue == null)
            {
                throw new ArgumentNullException();
            }
            PropertyValue = propertyValue;
        }

        public override List<string> GetNamespaces()
        {
            var namespaces = new List<string>();

            var valueType = PropertyValue.GetType();

            namespaces.Add(valueType.Namespace);

            if (valueType.GenericTypeArguments != null && valueType.GenericTypeArguments.Length > 0)
            {
                foreach (var type in valueType.GenericTypeArguments)
                {
                    namespaces.Add(type.Namespace);
                }
            }

            return namespaces;
        }

        public override string GetPropertyValue(
            bool isAutoProperty,
            bool hasPublicGetter,
            bool hasPublicSetter)
        {
            if (PropertyValue != null)
            {
                if (hasPublicGetter && !hasPublicSetter && !isAutoProperty)
                {
                    return $"=> {PrettyPrintValue(PropertyValue)};";
                }
                else if ((hasPublicGetter && hasPublicSetter) || isAutoProperty)
                {
                    return "{ get; set; } " + $"= {PrettyPrintValue(PropertyValue)};";
                }
                else
                {
                    return $"= {PrettyPrintValue(PropertyValue)};";
                }
            }
            else if (isAutoProperty)
            {
                return "{ get; set; }";
            }
            else if (hasPublicGetter)
            {
                return "{ get; }";
            }
            else if (hasPublicSetter)
            {
                return "{ set; }";
            }
            else
            {
                return ";";
            }
        }

        private string PrettyPrintValue(object value)
        {
            if (value is string)
            {
                return $"\"{value}\"";
            }
            if (value is char)
            {
                return $"'{value}'";
            }
            if (value is int || value is Int32)
            {
                return $"{value}";
            }
            if (value is long || value is Int64)
            {
                return $"{value}";
            }
            if (value is Uri)
            {
                return $"new Uri(\"{value.ToString()}\")";
            }
            if (value.GetType().IsEnum)
            {
                return PrettyPrintEnum(value);
            }
            if (value is List<string>)
            {
                return PrettyPrintList((List<string>)value);
            }
            if (value is List<ResponseTypeEnum>)
            {
                return PrettyPrintList((List<ResponseTypeEnum>)value);
            }
            if (value is Dictionary<HttpRequestHeader, string>)
            {
                return PrettyPrintDictionary((Dictionary<HttpRequestHeader, string>)value);
            }
            else
            {
                throw new Exception("Type was " + value.GetType().Name);
            }
        }

        private string PrettyPrintDictionary<TKey, TValue>(
            Dictionary<TKey, TValue> dictionary)
        {
            var keyType = typeof(TKey);
            var valueType = typeof(TValue);

            if (dictionary.Count == 0)
            {
                return "new Dictionary<" + keyType.Name + "," + valueType.Name + ">()";
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("new Dictionary<" + keyType.Name + "," + valueType.Name + "> { ");
            foreach (var item in dictionary)
            {
                stringBuilder.Append("{");
                if (keyType.IsEnum)
                {
                    stringBuilder.Append($"{keyType.Name}.{item.Key}, ");
                }
                else
                {
                    stringBuilder.Append($"\"{item.Key}\", ");
                }

                if (valueType.IsEnum)
                {
                    stringBuilder.Append($"{valueType.Name}.{item.Value}, ");
                }
                else
                {
                    stringBuilder.Append($"\"{item.Value}\" ");
                }
                stringBuilder.Append("}, ");
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append(" }");
            return stringBuilder.ToString();
        }

        private string PrettyPrintList<T>(List<T> values)
        {
            var genericType = typeof(T);

            if (values.Count == 0)
            {
                return "new List<" + genericType.Name + ">()";
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("new List<" + genericType.Name + "> { ");
            foreach (var item in values)
            {
                if (genericType.IsEnum)
                {
                    stringBuilder.Append($"{genericType.Name}.{item}, ");
                }
                else
                {
                    stringBuilder.Append($"\"{item}\", ");
                }
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append(" }");
            return stringBuilder.ToString();
        }

        private string PrettyPrintEnum(object item)
        {
            return $"{item.GetType().Name}.{item}";
        }
    }
}
