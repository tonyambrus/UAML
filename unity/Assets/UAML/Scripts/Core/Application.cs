using System;
using System.Reflection;
using Uaml.Events;
using Uaml.Internal.Reflection;
using Uaml.UX;

namespace Uaml.Core
{
    public static class Application
    {
        public static void LoadComponent(ElementBase element) => LoadComponent(element, element);

        private static void LoadComponent(ElementBase element, ElementBase root)
        {
            BindEvents(element, root);
            element.children.ForEach(child => LoadComponent(child, root));
        }

        private static void BindEvents(ElementBase element, ElementBase root)
        {
            var rootType = root.GetType();

            foreach (var pair in element.events)
            {
                var eventName = pair.Key;
                var bindingName = pair.Value;

                if (!EventManager.TryGetRoutedEvent(eventName, out var routedEvent))
                {
                    throw new Exception($"Failed to find event {eventName} in EventManager");
                }

                var ownerType = routedEvent.OwnerType;
                var ownerTypeInfo = ElementRegistry.GetElementType(ownerType);
                if (!ownerTypeInfo.hierarchyEvents.TryGetValue(eventName, out var eventInfo))
                {
                    throw new Exception($"Failed to bind event {eventName} on component {element.Name} to {bindingName}");
                }

                var rootMethodInfo = rootType.GetMethod(bindingName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (rootMethodInfo == null)
                {
                    throw new Exception($"Failed to find method {rootMethodInfo} on component {element.Name}");
                }

                var handler = (RoutedEventHandler)Delegate.CreateDelegate(typeof(RoutedEventHandler), root, rootMethodInfo);
                element.eventHandlers.AddHandler(routedEvent, handler);
            }
        }
    }
}
