using System;
using System.Collections.Generic;
using Uaml.Internal;

namespace Uaml.UX
{
    public class DependencyProperty
    {
        public delegate ref PropType Access<Owner, PropType>(Owner owner);

        public readonly string Name;
        public readonly Type PropertyType;
        public readonly Type OwnerType;
        public readonly PropertyMetadata PropertyMetadata;

        private GetDelegate getter;
        private SetDelegate setter;
        private bool initialized;

        private delegate void SetDelegate(object instance, object value);
        private delegate object GetDelegate(object instance);

        private static Dictionary<(string, Type), DependencyProperty> allProperties = new Dictionary<(string, Type), DependencyProperty>();
        private static Dictionary<Type, DependencyPropertySet> typeProperties = new Dictionary<Type, DependencyPropertySet>();

        internal static DependencyPropertySet GetDeclaredProperties(Type type) => typeProperties.TryGetValue(type, out var props) ? props : null;

        private DependencyProperty(string name, Type propertyType, Type ownerType, GetDelegate getter, SetDelegate setter, PropertyMetadata propertyMetadata)
        {
            Name = name;
            PropertyType = propertyType;
            OwnerType = ownerType;
            PropertyMetadata = propertyMetadata;
            this.getter = getter;
            this.setter = setter;
        }

        public static DependencyProperty Register<Owner, PropType>(string name, Access<Owner, PropType> accessor, PropertyMetadata propertyMetadata = null)
        {
            var ownerType = typeof(Owner);
            var propertyType = typeof(PropType);
            var getter = new GetDelegate((object obj) => accessor((Owner)obj));
            var setter = new SetDelegate((object obj, object value) => accessor((Owner)obj) = (PropType)value);

            var dp = new DependencyProperty(name, propertyType, ownerType, getter, setter, propertyMetadata);

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

        internal void SetValue(DependencyObject instance, object value)
        {
            var oldValue = getter(instance);
            if (oldValue != value)
            {
                instance?.NotifyPropertyChanged(this, oldValue, value);
                setter(instance, value);
            }
        }

        internal object GetValue(object instance) => getter?.Invoke(instance);

        internal static bool TryGetProperty(string name, Type ownerType, IEnumerable<string> namespaces, out DependencyProperty dp)
        {
            return Util.TryGetInTypeChain(ownerType, (name, ownerType), namespaces, allProperties.TryGetValue, out dp);
        }

        internal static bool HasProperty(string name, Type elementType, IEnumerable<string> namespaces)
        {
            return TryGetProperty(name, elementType, namespaces, out var _);
        }

        public override string ToString()
        {
            return $"DependencyProperty {OwnerType}.{Name} : {PropertyType}";
        }
    }
}