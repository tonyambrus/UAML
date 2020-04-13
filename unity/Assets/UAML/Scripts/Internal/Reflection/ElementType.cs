using System;

namespace Uaml.Internal.Reflection
{
    public class ElementType
    {
        public Type type;
        public ElementType baseClass;
        public PropertySet selfProps;
        public PropertySet hierarchyProps;

        public EventSet selfEvents;
        public EventSet hierarchyEvents;
    }
}