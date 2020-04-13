using System;
using System.Collections.Generic;
using System.Linq;
using Uaml.UX;

namespace Uaml.Events
{
    public static class EventManager
    {
        private static Dictionary<string, RoutedEvent> routedEvents = new Dictionary<string, RoutedEvent>(StringComparer.OrdinalIgnoreCase);

        public static RoutedEvent RegisterRoutedEvent(string name, RoutingStrategy strategy, Type handlerType, Type ownerType)
        {
            var routedEvent = new RoutedEvent(name, strategy, handlerType, ownerType);
            routedEvents[name] = routedEvent; // TODO: what if name is reused?
            return routedEvent;
        }

        internal static bool TryGetRoutedEvent(string name, out RoutedEvent routedEvent) => routedEvents.TryGetValue(name, out routedEvent);
        internal static bool HasRoutedEvent(string name) => routedEvents.ContainsKey(name);

        internal static void RaiseEvent(ElementBase source, RoutedEvent routedEvent)
        {
            var args = new RoutedEventArgs(routedEvent, source);
            var chain = GetParentChain(source);

            if (routedEvent.RoutingStrategy == RoutingStrategy.Direct)
            {
                chain = chain.Take(1);
            }
            else if (routedEvent.RoutingStrategy == RoutingStrategy.Tunnel)
            {
                chain = chain.Reverse();
            }

            foreach (var element in chain)
            {
                element.HandleEvent(args);

                if (args.Handled)
                {
                    break;
                }
            }
        }

        private static IEnumerable<ElementBase> GetParentChain(ElementBase element)
        {
            yield return element;

            while (element.parent)
            {
                element = element.parent;
                yield return element;
            }
        }
    }
}