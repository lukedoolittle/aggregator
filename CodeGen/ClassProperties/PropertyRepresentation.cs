using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeGen
{
    public class PropertyRepresentation
    {
        public string Comments { get; set; }
        public string AccessModifier => GetAccessModifier();
        public string Modifier => GetModifier();

        public string Name { get; }
        public string TypeName => GetTypename();
        public List<string> Namespaces => GetNamespaces();

        public string Value
        {
            get
            {
                if (PropertyValue != null)
                {
                    return PropertyValue.GetPropertyValue(
                        IsAutoProperty,
                        HasPublicGetter,
                        HasPublicSetter);
                }
                else if (IsAutoProperty)
                {
                    return "{ get; set; }";
                }
                else if (HasPublicGetter)
                {
                    return "{ get; }";
                }
                else if (HasPublicSetter)
                {
                    return "{ set; }";
                }
                else
                {
                    return ";";
                }
            }
        }

        public List<MetadataRepresentation> Metadatas { get; set; } =
            new List<MetadataRepresentation>();

        public bool IsPublic { get; set; } = true;
        public bool IsProtected { get; set; } = false;
        public bool IsPrivate { get; set; } = false;

        public bool IsInternal { get; set; } = false;
        public bool IsVirtual { get; set; } = false;
        public bool IsAbstract { get; set; } = false;
        public bool IsOverride { get; set; } = false;

        private readonly Type _type;
        private readonly string _typeName;
        private readonly string _typeNamespace;

        public bool IsAutoProperty { get; set; } = false;
        public bool HasPublicGetter { get; set; } = true;
        public bool HasPublicSetter { get; set; } = false;
        public ValueRepresentation PropertyValue { get; set; } = null;


        public PropertyRepresentation(
            Type type,
            string name)
        {
            _type = type;
            Name = name;
        }

        public PropertyRepresentation(
            string typeName,
            string typeNamespace,
            string name)
        {
            _typeName = typeName;
            _typeNamespace = typeNamespace;
            Name = name;
        }

        private string GetModifier()
        {
            if (IsVirtual)
            {
                return "virtual";
            }
            if (IsAbstract)
            {
                return "abstract";
            }
            if (IsOverride)
            {
                return "override";
            }
            return string.Empty;
        }

        private string GetAccessModifier()
        {
            if (IsPublic)
            {
                return "public";
            }
            if (IsProtected)
            {
                return "protected";
            }
            throw new Exception();
        }

        private string GetTypename()
        {
            if (_type == null)
            {
                return _typeName;
            }
            else
            {
                return PrettyPrintTypename(_type);
            }
        }

        private List<string> GetNamespaces()
        {
            var namespaces = new List<string>();

            if (_type == null)
            {
                namespaces.Add(_typeNamespace);
            }
            else
            {
                namespaces.Add(_type.Namespace);

                if (_type.GenericTypeArguments != null && _type.GenericTypeArguments.Length > 0)
                {
                    foreach (var type in _type.GenericTypeArguments)
                    {
                        namespaces.Add(type.Namespace);
                    }
                }
            }

            foreach (var metadata in Metadatas)
            {
                namespaces.AddRange(metadata.Namespace);
            }

            if (PropertyValue != null)
            {
                namespaces.AddRange(PropertyValue.GetNamespaces());
            }

            return namespaces;
        }

        private string PrettyPrintTypename(Type instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            if (!instance.GetTypeInfo().IsGenericType)
            {
                return instance.Name;
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(instance.Name.Substring(0, instance.Name.LastIndexOf("`", StringComparison.Ordinal)));
            stringBuilder.Append(instance.GetTypeInfo().GenericTypeArguments.Aggregate("<",
                (aggregate, type) => aggregate + (aggregate == "<" ? "" : ",") + PrettyPrintTypename(type)));
            stringBuilder.Append(">");

            return stringBuilder.ToString();
        }
    }
}
