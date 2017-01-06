namespace CodeGen
{
    public class BoxedMetadata
    {
        public string Type { get; }
        public string Arguments { get; }

        public BoxedMetadata(string type, string arguments)
        {
            Type = type;
            Arguments = arguments;
        }
    }
}
