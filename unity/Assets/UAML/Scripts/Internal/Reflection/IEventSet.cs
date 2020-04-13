using System.Collections.Generic;
using System.Reflection;

namespace Uaml.Internal.Reflection
{
    public interface IEventSet : IReadOnlyDictionary<string, EventInfo> { }
}