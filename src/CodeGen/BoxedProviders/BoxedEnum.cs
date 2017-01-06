using System.Collections.Generic;

namespace CodeGen
{
    public class BoxedEnum
    {
        public string Name { get; }
        public IDictionary<string, string> Items { get; }

        public BoxedEnum(string name, IDictionary<string, string> items)
        {
            Name = name;
            Items = items;
        }
    }
}
