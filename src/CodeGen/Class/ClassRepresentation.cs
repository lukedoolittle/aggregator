using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeGen.Class;

namespace CodeGen
{
//handle generic type constraints
    public class ClassRepresentation
    {
        public string Comments { get; set; }

        public List<string> GetNamespaces()
        {
            var namespaces = new List<string>();

            namespaces.AddRange(
                Metadatas.Select(m => m.Namespace).SelectMany(a => a).ToList());

            namespaces.AddRange(
                Properties.Select(p => p.Namespaces).SelectMany(a => a).ToList());

            if (BaseType != null)
            {
                namespaces.Add(BaseType.Namespace);
            }

            namespaces.AddRange(
                InterfaceTypes.Select(i => i.Namespace));

            namespaces.AddRange(
                Enums.Select(e => e.Namespace));

            return namespaces.Distinct().ToList();
        }

        //change to property
        public string GetBase()
        {
            if (BaseType == null && InterfaceTypes.Count == 0)
            {
                return null;
            }

            var stringBuilder = new StringBuilder();

            if (BaseType != null)
            {
                stringBuilder.Append(BaseType.TypeName + ", ");
            }

            foreach (var @interface in InterfaceTypes)
            {
                stringBuilder.Append(@interface.Name + ", ");
            }

            return stringBuilder.ToString().TrimEnd().TrimEnd(',');
        }

        public string AccessModifier
        {
            get
            {
                if (IsPublic)
                {
                    return "public";
                }
                if (IsInternal)
                {
                    return "internal";
                }
                throw new Exception();
            }
        }

        public List<MetadataRepresentation> Metadatas { get; set; } =
            new List<MetadataRepresentation>();
        public List<string> OpenGenericTypeNames { get; set; } = 
            new List<string>(); 
        public List<EnumRepresentation> Enums { get; set; } = 
            new List<EnumRepresentation>();
        public bool IsPublic { get; set; } = true;
        public bool IsInternal { get; set; } = false;

        public BaseTypeRepresentation BaseType { get; set; } = null;
        public List<Type> InterfaceTypes { get; set; } = new List<Type>(); 

        public string Name { get; set; }
        public string Namespace { get; set; }

        public List<PropertyRepresentation> Properties { get; set; } =
            new List<PropertyRepresentation>();

        public ClassRepresentation(string name, string @namespace)
        {
            Name = name;
            Namespace = @namespace;
        }


    }
}
