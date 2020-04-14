using System;
using System.Collections.Generic;
using System.Reflection;
using Uaml.Internal;

namespace Uaml.UX
{
    public class DependencyProperty
    {
        public string Name { get; private set; }
        public Type PropertyType { get; private set; }
        public Type OwnerType { get; private set; }

        private bool initialized;
        private GetDelegate getter;
        private SetDelegate setter;

        private delegate void SetDelegate(object instance, object value);
        private delegate object GetDelegate(object instance);

        private static Dictionary<(string, Type), DependencyProperty> allProperties = new Dictionary<(string, Type), DependencyProperty>();
        private static Dictionary<Type, DependencyPropertySet> typeProperties = new Dictionary<Type, DependencyPropertySet>();

        internal static DependencyPropertySet GetDeclaredProperties(Type type) => typeProperties.TryGetValue(type, out var props) ? props : null;

        internal PropertyInfo Property { get; private set; }

        internal void SetValue(object instance, object value)
        {
            InitAccessors();
            setter?.Invoke(instance, value);
        }

        internal object GetValue(object instance)
        {
            InitAccessors();
            return getter?.Invoke(instance);
        }

        private void InitAccessors()
        {
            if (!initialized)
            {
                initialized = true;

                Property = OwnerType.GetProperty(Name);
                if (Property != null)
                {
                    if (Property.CanWrite)
                    {
                        setter = Property.SetValue;
                    }

                    if (Property.CanRead)
                    {
                        getter = Property.GetValue;
                    }
                }
            }
        }

        internal static bool HasProperty(string name, Type ownerType, bool declaredOnly = false)
        {
            if (declaredOnly)
            {
                return allProperties.ContainsKey((name, ownerType));
            }
            else
            {
                return Util.WalkTypeChain(ownerType, type => allProperties.ContainsKey((name, type)));
            }
        }

        public static DependencyProperty Register(string name, Type propertyType, Type ownerType)
        {
            var dp = new DependencyProperty
            {
                Name = name,
                PropertyType = propertyType,
                OwnerType = ownerType
            };

            if (!allProperties.ContainsKey((name, ownerType)))
            {
                allProperties[(name, ownerType)] = dp;
            }
            else
            {
                throw new Exception($"Already added DependencyProperty ({name},{propertyType},{ownerType})");
            }

            if (!typeProperties.TryGetValue(ownerType, out var typeProperty))
            {
                typeProperty = new DependencyPropertySet(ownerType);
                typeProperties[ownerType] = typeProperty;
            }
            typeProperty[name] = dp;

            return dp;
        }
    }
}