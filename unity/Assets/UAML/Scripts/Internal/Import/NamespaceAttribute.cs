namespace Uaml.Internal
{
    public struct NamespaceAttribute
    {
        private const string namespaceKeyPrefix = "xmlns";
        private const string namespacePrefix = "clr-namespace:";

        public Attribute Attribute { get; private set; }
        public string Namespace { get; private set; }
        public string Url { get; private set; }

        public static bool IsNamespaceKey(Attribute attribute) => attribute.Key == namespaceKeyPrefix;
        public static bool IsCLRNamespace(Attribute attribute) => attribute.Value.StartsWith(namespacePrefix);

        public static Attribute Parse(Attribute attribute)
        {
            return new Attribute(attribute.Key, attribute.Value.Substring(namespacePrefix.Length));
        }
    }
}