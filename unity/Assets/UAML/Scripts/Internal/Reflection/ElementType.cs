using System;
using Uaml.Internal.Events;

namespace Uaml.Internal.Reflection
{
    internal class ElementType
    {
        public Type type;
        public ElementType baseClass;
        public DependencyPropertySet selfProps;
        public DependencyPropertySet hierarchyProps;

        public RoutedEventSet selfEvents;
        public RoutedEventSet hierarchyEvents;
    }
}