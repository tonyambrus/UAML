using System;
using System.Collections.Generic;

namespace Uaml.Internal.Data
{
    public class Element
    {
        public string name;
        public string className; // can be different from type if it's not been generated yet
        public Type type;
        public Dictionary<string, string> rawAttributes = new Dictionary<string, string>();
        public Dictionary<string, string> properties = new Dictionary<string, string>();
        public Dictionary<string, string> events = new Dictionary<string, string>();
        public List<Element> children = new List<Element>();
    }
}
