using System.Collections.Generic;
using System.Reflection;

namespace Uaml.Internal.Reflection
{
    public interface IPropertySet : IReadOnlyDictionary<string, PropertyInfo> { }
}