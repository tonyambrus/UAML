using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uaml.UX;

namespace Uaml.Internal.Reflection
{ 
    public static class ElementRegistry
    {
        private static Dictionary<Type, ElementType> registry = new Dictionary<Type, ElementType>();

        private static bool DescendsFromElement(Type type) => typeof(ElementBase).IsAssignableFrom(type);

        public static ElementType GetElementType(Type type)
        {
            if (!DescendsFromElement(type))
            {
                return null;
            }

            if (!registry.TryGetValue(type, out var info))
            {
                var selfFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;

                info = new ElementType
                {
                    type = type,

                    hierarchyProps = ToSet(type.GetProperties()),
                    selfProps = ToSet(type.GetProperties(selfFlags)),

                    hierarchyEvents = ToSet(type.GetEvents()),
                    selfEvents = ToSet(type.GetEvents(selfFlags)),

                    baseClass = type == typeof(ElementBase) ? null : GetElementType(type.BaseType)
                };

                registry[type] = info;
            }

            return info;
        }

        private static Dictionary<string, T> ToDict<T>(IEnumerable<T> props) where T : MemberInfo
            => props.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

        private static PropertySet ToSet(PropertyInfo[] props)
        {
            var dict = props
                .Where(p => typeof(ElementBase).IsAssignableFrom(p.DeclaringType))
                .Where(p => p.CanRead && p.CanWrite)
                .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

            return new PropertySet(dict);
        }

        private static EventSet ToSet(EventInfo[] events)
        {
            var dict = events
                .Where(p => typeof(ElementBase).IsAssignableFrom(p.DeclaringType))
                .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

            return new EventSet(dict);
        }

        public static PropertySet GetProperties(Type type) => GetElementType(type).hierarchyProps;
        public static PropertySet GetDeclaredProperties(Type type) => GetElementType(type).selfProps;
        public static EventSet GetEvents(Type type) => GetElementType(type).hierarchyEvents;
        public static EventSet GetDeclaredEvents(Type type) => GetElementType(type).selfEvents;
    }
}