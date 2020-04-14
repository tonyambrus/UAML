using System;
using System.Reflection;

namespace Uaml.Events
{
    public delegate void RoutedEventHandler(object sender, RoutedEventArgs e);

    public sealed class RoutedEvent
    {
        internal RoutedEvent(string name, RoutingStrategy strategy, Type handlerType, Type ownerType)
        {
            Name = name;
            RoutingStrategy = strategy;
            HandlerType = handlerType;
            OwnerType = ownerType;
        }

        private bool initialized;
        private EventInfo eventInfo;

        internal EventInfo EventInfo
        {
            get
            {
                InitializeAccessors();
                return eventInfo;
            }
        }

        private void InitializeAccessors()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;

            eventInfo = OwnerType.GetEvent(Name);
        }

        //
        // Summary:
        //     Gets the identifying name of the routed event.
        //
        // Returns:
        //     The name of the routed event.
        public string Name { get; }
        //
        // Summary:
        //     Gets the routing strategy of the routed event.
        //
        // Returns:
        //     One of the enumeration values. The default is the enumeration default, System.Windows.RoutingStrategy.Bubble.
        public RoutingStrategy RoutingStrategy { get; }
        //
        // Summary:
        //     Gets the handler type of the routed event.
        //
        // Returns:
        //     The handler type of the routed event.
        public Type HandlerType { get; }
        //
        // Summary:
        //     Gets the registered owner type of the routed event.
        //
        // Returns:
        //     The owner type of the routed event.
        public Type OwnerType { get; }

        //
        // Summary:
        //     Associates another owner type with the routed event represented by a System.Windows.RoutedEvent
        //     instance, and enables routing of the event and its handling.
        //
        // Parameters:
        //   ownerType:
        //     The type where the routed event is added.
        //
        // Returns:
        //     The identifier field for the event. This return value should be used to set a
        //     public static read-only field that will store the identifier for the representation
        //     of the routed event on the owning type. This field is typically defined with
        //     public access, because user code must reference the field in order to attach
        //     any instance handlers for the routed event when using the System.Windows.UIElement.AddHandler(System.Windows.RoutedEvent,System.Delegate,System.Boolean)
        //     utility method.
        public RoutedEvent AddOwner(Type ownerType)
        {
            throw new NotImplementedException();
        }

        //
        // Summary:
        //     Returns the string representation of this System.Windows.RoutedEvent.
        //
        // Returns:
        //     A string representation for this object, which is identical to the value returned
        //     by System.Windows.RoutedEvent.Name.
        //public override string ToString();
    }
}