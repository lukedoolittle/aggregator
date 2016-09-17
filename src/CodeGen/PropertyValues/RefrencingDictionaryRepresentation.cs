using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CodeGen
{
    public class ReferencingDictionaryRepresentation : ValueRepresentation
    {
        private readonly Dictionary<string, string> _dictionary =
            new Dictionary<string, string>();

        public void Add(string key, string value)
        {
            _dictionary.Add(key, value);
        }

        public override List<string> GetNamespaces()
        {
            return new List<string>
            {
                typeof(Dictionary<string, string>).Namespace,
                typeof(ReadOnlyDictionary<string, string>).Namespace
            };
        }

        public override string GetPropertyValue(
            bool isAutoProperty,
            bool hasPublicGetter,
            bool hasPublicSetter)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(" => new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { ");
            foreach (var item in _dictionary)
            {
                stringBuilder.Append("{");
                stringBuilder.Append($"\"{item.Key}\", {item.Value}?.ToString()");
                stringBuilder.Append("}, ");
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append(" });");
            return stringBuilder.ToString();
        }
    }
}
