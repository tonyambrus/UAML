using System;
using System.Collections.Generic;
using System.Linq;
using Uaml.Internal.Events;
using Uaml.UX;

namespace Uaml.Events
{
    public static class EventManager
    {
        private static Dictionary<(string, Type), RoutedEvent> allEvents = new Dictionary<(string, Type), RoutedEvent>();
        private static Dictionary<Type, RoutedEventSet> typeEvents = new Dictionary<Type, RoutedEventSet>();
        private static Dictionary<string, Type> typeOwners = new Dictionary<string, Type>();

        public static RoutedEvent RegisterRoutedEvent(string name, RoutingStrategy strategy, Type handlerType, Type ownerType)
        {
            var routedEvent = new RoutedEvent(name, strategy, handlerType, ownerType);

            if (!allEvents.ContainsKey((name, ownerType)))
            {
                allEvents[(name, ownerType)] = routedEvent; // TODO: what if name is reused?
            }
            else
            {
                throw new Exception($"Routed Event ({name}, {ownerType}) already exists");
            }

            if (!typeEvents.TryGetValue(ownerType, out var typeEvent))
            {
                typeEvent = new RoutedEventSet(ownerType);
                typeEvents[ownerType] = typeEvent;
            }

            typeEvent[name] = routedEvent;

            if (!typeOwners.TryGetValue(ownerType.Name, out var type))
            {
                typeOwners[ownerType.Name] = ownerType;
            }
            else
            {
                throw new Exception($"Type {ownerType.FullName} conflicts with {type.FullName}");
            }

            return routedEvent;
        }

        internal static void RaiseEvent(FrameworkElement source, RoutedEvent routedEvent)
        {
            var args = new RoutedEventArgs(routedEvent, source);
            var chain = source.ElementChain;

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

        internal static RoutedEventSet GetDeclaredEvents(Type type) => typeEvents.TryGetValue(type, out var events) ? events : null;

        internal static bool HasRoutedEvent(string name, Type elementType, IEnumerable<string> namespaces)
        {
            return TryGetRoutedEvent(name, elementType, namespaces, out var _);
        }

        internal static bool TryGetRoutedEvent(string name, Type elementType, IEnumerable<string> namespaces, out RoutedEvent routedEvent)
        {
            if (Util.TryParseQualifiedName(name, elementType, namespaces, out var propertyName, out var ownerType))
            {
                if (allEvents.TryGetValue((propertyName, ownerType), out routedEvent))
                {
                    return true;
                }
            }

            routedEvent = default;
            return false;
        }
    }
}