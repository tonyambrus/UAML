using System.Collections.Generic;
using Uaml.Events;
using Uaml.UX;
using UnityEngine.Events;

namespace Uaml.Internal
{
    internal class EventHandlers
    {
        private Dictionary<(UnityEvent source, RoutedEvent routedEvent), UnityAction> registeredEvents = null;
        private Dictionary<RoutedEvent, RoutedEventHandler> eventHandlers = null;

        internal void AddHandler(RoutedEvent routedEvent, RoutedEventHandler handler)
        {
            if (eventHandlers == null)
            {
                eventHandlers = new Dictionary<RoutedEvent, RoutedEventHandler>();
            }

            if (eventHandlers.TryGetValue(routedEvent, out var currentHandler))
            {
                handler = currentHandler + handler;
            }

            eventHandlers[routedEvent] = handler;
        }

        internal void RemoveHandler(RoutedEvent routedEvent, RoutedEventHandler handler)
        {
            if (eventHandlers == null)
            {
                return;
            }

            if (eventHandlers.TryGetValue(routedEvent, out var currentHandler))
            {
                handler = currentHandler - handler;
            }

            if (handler != null)
            {
                eventHandlers[routedEvent] = handler;
            }
            else
            {
                eventHandlers.Remove(routedEvent);
            }

            if (eventHandlers.Count == 0)
            {
                eventHandlers = null;
            }
        }

        internal void HandleEvent(object sender, RoutedEventArgs args)
        {
            if (eventHandlers == null)
            {
                return;
            }

            if (eventHandlers.TryGetValue(args.RoutedEvent, out var handler))
            {
                handler.Invoke(sender, args);
            }
        }

        internal void BindEvent(ElementBase self, UnityEvent source, RoutedEvent routedEvent)
        {
            var sender = this;
            var action = new UnityAction(() => EventManager.RaiseEvent(self, routedEvent));

            source.AddListener(action);

            if (registeredEvents == null)
            {
                registeredEvents = new Dictionary<(UnityEvent source, RoutedEvent routedEvent), UnityAction>();
            }

            registeredEvents[(source, routedEvent)] = action;
        }

        internal void UnbindEvent(UnityEvent source, RoutedEvent routedEvent)
        {
            var key = (source, routedEvent);
            if (registeredEvents != null && registeredEvents.TryGetValue(key, out var action))
            {
                source.RemoveListener(action);
                registeredEvents.Remove(key);
            }
        }
    }
}