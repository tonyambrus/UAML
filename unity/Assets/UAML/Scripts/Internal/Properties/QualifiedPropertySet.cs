using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Uaml.UX;

namespace Uaml.Internal
{
    internal class QualifiedPropertySet<T> : IEnumerable<KeyValuePair<string, T>>
    {
        private Dictionary<string, T> properties = new Dictionary<string, T>();

        public Type OwnerType { get; private set; }

        public int Count => properties.Count;

        public QualifiedPropertySet(Type ownerType) : this(ownerType, new Dictionary<string, T>())
        {
        }

        public QualifiedPropertySet(Type ownerType, Dictionary<string, T> dict)
        {
            this.OwnerType = ownerType;
            this.properties = dict;
        }

        public bool ContainsKey(string qualifiedName, IEnumerable<string> namespaces)
        {
            return Util.TryParseQualifiedName(qualifiedName, OwnerType, namespaces, out var name, out var ownerType) && properties.ContainsKey(name);
        }

        public bool TryGetValue(string qualifiedName, IEnumerable<string> namespaces, out T property)
        {
            property = default;
            return Util.TryParseQualifiedName(qualifiedName, OwnerType, namespaces, out var name, out var ownerType) && properties.TryGetValue(name, out property);
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => properties.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => properties.GetEnumerator();

        internal T this[string name]
        {
            get => properties[name];
            set => properties[name] = value;
        }
    }
}