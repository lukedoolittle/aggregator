using System.Collections.Generic;

namespace CodeGen.Class
{
    public class EnumRepresentation
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public Dictionary<string, string> Values { get; set; } = 
            new Dictionary<string, string>();

        public EnumRepresentation(
            string name, 
            string @namespace)
        {
            Name = name;
            Namespace = @namespace;
        }
    }
}
