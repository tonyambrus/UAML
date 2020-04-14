namespace Uaml.Internal
{
    public struct Attribute
    {
        public string Key { get; private set; }
        public string Alias { get; private set; }
        public string Value { get; private set; }

        public Attribute(string key, string value)
        {
            var parts = key.Split(':');
            Key = parts[0];
            Alias = (parts.Length > 1) ? parts[1] : null;
            Value = value;
        }
    }
}