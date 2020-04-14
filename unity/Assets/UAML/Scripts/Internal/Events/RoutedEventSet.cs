using System;
using System.Collections.Generic;
using Uaml.Events;

namespace Uaml.Internal.Events
{
    internal class RoutedEventSet : QualifiedPropertySet<RoutedEvent>
    {
        public RoutedEventSet(Type ownerType)
            : base(ownerType)
        {
        }

        public RoutedEventSet(Type ownerType, IDictionary<string, RoutedEvent> dict)
            : base(ownerType, new Dictionary<string, RoutedEvent>(dict))
        {
        }
    }
}