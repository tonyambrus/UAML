using System.Collections.Generic;
using System.Reflection;

namespace Uaml.Internal.Reflection
{
    public class PropertySet : Dictionary<string, PropertyInfo>, IPropertySet
    {
        public PropertySet(IDictionary<string, PropertyInfo> dictionary) : base(dictionary) { }
    }
}