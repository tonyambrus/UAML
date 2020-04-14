using System;
using System.Collections.Generic;

namespace Uaml.Internal.Data
{
    public class Element
    {
        public Element parent;

        public string name;
        public string className; // can be different from type if it's not been generated yet
        public Type type;
        public Dictionary<string, Attribute> rawAttributes = new Dictionary<string, Attribute>();
        public Dictionary<string, Attribute> properties = new Dictionary<string, Attribute>();
        public List<string> namespaces = new List<string>();
        public Dictionary<string, Attribute> events = new Dictionary<string, Attribute>();
        public List<Element> children = new List<Element>();
    }
}
