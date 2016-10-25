using System;
using System.Collections.Generic;
using System.Text;
using Material.Enums;

namespace CodeGen.Metadata
{
    public class ConcreteMetadataRepresentation : MetadataRepresentation
    {
        public override string TypeName => Type.Name;
        public override List<string> Namespace => GetNamespaces();
        public override string ConstructorArguments => GetConstructorArguments();

        public Type Type { get; set; }

        public List<object> ConstructorParameters { get; set; }

        public ConcreteMetadataRepresentation(Type type)
        {
            Type = type;
        }

        private List<string> GetNamespaces()
        {
            var namespaces = new List<string>();

            namespaces.Add(Type.Namespace);

            if (Type.GenericTypeArguments != null && Type.GenericTypeArguments.Length > 0)
            {
                foreach (var type in Type.GenericTypeArguments)
                {
                    namespaces.Add(type.Namespace);
                }
            }

            foreach (var item in ConstructorParameters)
            {
                if (item is Type)
                {
                    namespaces.Add(typeof(Type).Namespace);
                    namespaces.Add(((Type)item).Namespace);
                }
                else
                {
                    namespaces.Add(item.GetType().Namespace);
                }

            }

            return namespaces;
        }

        private string GetConstructorArguments()
        {
            var stringBuilder = new StringBuilder();

            if (ConstructorParameters == null || ConstructorParameters.Count == 0)
            {
                return string.Empty;
            }

            foreach (var item in ConstructorParameters)
            {
                if (item is Type)
                {
                    stringBuilder.Append($"typeof({((Type)item).Name}), ");
                }
                else if (item is string)
                {
                    stringBuilder.Append($"\"{item.ToString()}\", ");
                }
                else if (item is RequestParameterType)
                {
                    stringBuilder.Append($"RequestParameterType.{item.ToString()}, ");
                }
                else
                {
                    throw new Exception("Dont know how to handle this constructor parameter");
                }
            }

            var result = stringBuilder.ToString();
            return result.Trim(' ').Trim(',');
        }
    }
}
