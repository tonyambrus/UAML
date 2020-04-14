using System;
using System.Collections.Generic;

namespace Uaml.UX
{
    public static class Util
    {
        public static bool TryParseQualifiedName(string qualifiedName, Type elementType, IEnumerable<string> namespaces, out string propertyName, out Type type)
        {
            var index = qualifiedName.LastIndexOf('.');
            if (index == -1)
            {
                propertyName = qualifiedName;
                type = elementType;
                return true;
            }
            else
            {
                var typeName = qualifiedName.Substring(0, index);
                propertyName = qualifiedName.Substring(index + 1);

                type = Type.GetType(typeName, throwOnError: false);
                if (type != null)
                {
                    return true;
                }

                if (namespaces != null)
                {
                    foreach (var ns in namespaces)
                    {
                        type = Type.GetType($"{ns}.{typeName}", throwOnError: false);
                        if (type != null)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        internal static bool IsFrameworkElementType(Type type) => typeof(FrameworkElement).IsAssignableFrom(type);

        internal static IEnumerable<Type> GetTypeChain(Type type, bool selfOnly = false)
        {
            if (!IsFrameworkElementType(type))
            {
                throw new Exception($"OwnerType {type} must derive from FrameworkElement");
            }

            while (type != null)
            {
                yield return type;

                if (selfOnly || type == typeof(FrameworkElement))
                {
                    break;
                }

                type = type.BaseType;
            }
        }

        internal static bool WalkTypeChain(Type type, Predicate<Type> predicate)
        {
            foreach (var t in GetTypeChain(type))
            {
                if (predicate(type))
                {
                    return true;
                }
            }

            return false;
        }
    }
}