using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uaml.Events;
using Uaml.Internal.Events;
using Uaml.UX;

namespace Uaml.Internal.Reflection
{ 
    public static class ElementRegistry
    {
        private static Dictionary<Type, ElementType> registry = new Dictionary<Type, ElementType>();

       
        internal static ElementType GetElementType(Type type)
        {
            if (!Util.IsFrameworkElementType(type))
            {
                throw new Exception($"Type {type} is not a FrameworkElement type");
            }

            if (!registry.TryGetValue(type, out var info))
            {
                info = new ElementType
                {
                    type = type,

                    hierarchyProps = FindProperties(type, declaredOnly: false),
                    selfProps = FindProperties(type, declaredOnly: true),

                    hierarchyEvents = FindEvents(type, declaredOnly: false),
                    selfEvents = FindEvents(type, declaredOnly: true),

                    baseClass = type == typeof(FrameworkElement) ? null : GetElementType(type.BaseType)
                };

                registry[type] = info;
            }

            return info;
        }

        private static Dictionary<string, T> ToDict<T>(IEnumerable<T> props) where T : MemberInfo
            => props.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

        private static DependencyPropertySet FindProperties(Type type, bool declaredOnly)
        {
            var dict = Util
                .GetTypeChain(type, declaredOnly)
                .Select(t => DependencyProperty.GetDeclaredProperties(t))
                .Where(p => p != null)
                .SelectMany(p => p)
                .ToDictionary(p => p.Key, p => p.Value, StringComparer.OrdinalIgnoreCase);

            return new DependencyPropertySet(type, dict);
        }

        private static RoutedEventSet FindEvents(Type type, bool declaredOnly)
        {
            var dict = Util
                .GetTypeChain(type, declaredOnly)
                .Select(t => EventManager.GetDeclaredEvents(t))
                .Where(p => p != null)
                .SelectMany(p => p)
                .ToDictionary(p => p.Key, p => p.Value, StringComparer.OrdinalIgnoreCase);

            return new RoutedEventSet(type, dict);
        }

        internal static DependencyPropertySet GetProperties(Type type) => GetElementType(type).hierarchyProps;
        internal static DependencyPropertySet GetDeclaredProperties(Type type) => GetElementType(type).selfProps;
        internal static RoutedEventSet GetEvents(Type type) => GetElementType(type).hierarchyEvents;
        internal static RoutedEventSet GetDeclaredEvents(Type type) => GetElementType(type).selfEvents;
    }
}