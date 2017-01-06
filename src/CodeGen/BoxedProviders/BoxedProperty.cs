using System.Collections.Generic;

namespace CodeGen
{
    public class BoxedProperty
    {
        public string Comment { get; }
        public string Type { get; }
        public string Name { get; }
        public string Value { get; }
        public List<BoxedMetadata> Metadatas { get; }
        public BoxedEnum AssociatedEnum { get; }

        public BoxedProperty(
            string comment, 
            string type,
            string name, 
            string value,
            List<BoxedMetadata> metadatas,
            BoxedEnum associatedEnum)
        {
            Comment = comment;
            Type = type;
            Name = name;
            Value = value;
            Metadatas = metadatas;
            AssociatedEnum = associatedEnum;
        }
    }
}
