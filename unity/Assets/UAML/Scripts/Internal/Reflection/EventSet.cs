using System.Collections.Generic;
using System.Reflection;

namespace Uaml.Internal.Reflection
{
    public class EventSet : Dictionary<string, EventInfo>, IEventSet
    {
        public EventSet(IDictionary<string, EventInfo> dictionary) : base(dictionary) { }
    }
}