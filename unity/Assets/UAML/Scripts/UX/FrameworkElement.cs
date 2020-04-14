using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uaml.Core;
using Uaml.Events;
using Uaml.Internal;
using Uaml.Internal.Events;
using Uaml.Internal.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Uaml.UX
{

    public class FrameworkElement : MonoBehaviour
    {
        public static readonly DependencyProperty DataContextProperty = DependencyProperty.Register("DataContext", typeof(object), typeof(FrameworkElement));

        [SerializeField] private StringSerializableDictionary properties = new StringSerializableDictionary();
        [SerializeField] private StringSerializableDictionary events = new StringSerializableDictionary();
        [SerializeField] private List<string> namespaces = new List<string>();
        [SerializeField] private List<FrameworkElement> children = new List<FrameworkElement>();
        [SerializeField] private FrameworkElement parent;

        public virtual bool IsRoot => false;
        
        public FrameworkElement RootElement => parent ? parent.RootElement : this;
        public FrameworkElement Parent => parent;
        public IReadOnlyList<FrameworkElement> Children => children;
        public object DataContext { get; set; }

        protected virtual void Awake() { }

        #region Events
        internal EventHandlers eventHandlers = new EventHandlers();

        protected void AddHandler(RoutedEvent routedEvent, RoutedEventHandler handler) => eventHandlers.AddHandler(routedEvent, handler);
        protected void RemoveHandler(RoutedEvent routedEvent, RoutedEventHandler handler) => eventHandlers.RemoveHandler(routedEvent, handler);
        protected void BindEvent(UnityEvent source, RoutedEvent routedEvent) => eventHandlers.BindEvent(this, source, routedEvent);
        protected void UnbindEvent(UnityEvent source, RoutedEvent routedEvent) => eventHandlers.UnbindEvent(source, routedEvent);

        internal void HandleEvent(RoutedEventArgs args) => eventHandlers.HandleEvent(this, args);
        
        #endregion Events

        #region Visibility
        protected bool showSelf = true;
        protected bool showInHierarchy = true;

        public bool ShowSelf
        {
            get => showSelf;
            set
            {
                if (showSelf != value)
                {
                    showSelf = value;
                    UpdateShow();
                }
            }
        }

        private bool ShowInHierarchy
        {
            get => (showInHierarchy || IsRoot) && showSelf;
            set
            {
                if (showInHierarchy != value)
                {
                    showInHierarchy = value;
                    UpdateShow();
                }
            }
        }

        private void UpdateShow()
        {
            children.ForEach(c => { if (c) c.ShowInHierarchy = ShowInHierarchy; });
            instance.gameObject.hideFlags = ShowInHierarchy ? HideFlags.None : HideFlags.HideInHierarchy;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.DirtyHierarchyWindowSorting();
#endif
        }
        #endregion Visibility

        #region Initialization
        [SerializeField] internal Component instance;
        [SerializeField] internal Transform container;
        [SerializeField] internal string Name;
        [SerializeField] internal Schema schema;

        public void SetInstance(Component instance)
        {
            this.instance = instance;
            this.container = schema.GetContainerForInstance(Name, instance);

            if (IsRoot)
            {
                this.instance.transform.SetParent(transform, false);
                this.instance.name = "_" + Name;
            }
        }

        public void AddChild(FrameworkElement element)
        {
            if (!container)
            {
                throw new Exception($"Element {Name} can't have children");
            }

            children.Add(element);
            element.parent = this;
            element.transform.SetParent(transform, false);
            element.instance.transform.SetParent(container, false);
            element.ShowInHierarchy = ShowInHierarchy;
            element.ShowSelf = false;

            const int elementPrefix = 0xF000;
            if (element.name.Length == 0 || element.name[0] < elementPrefix)
            {
                // We must have a unique identifer for path for Unity AssetImportContext to use
                // so just prepend a unique non-printable unicode character.
                var id = elementPrefix + (children.Count - 1);
                element.name = $"{char.ConvertFromUtf32(id)}{element.Name}";
            }

            element.instance.name = $"_{element.name}";
        }

        internal void BindProperties(bool includeChildren = true, bool onlyLiterals = false)
        {
            var propSet = ElementRegistry.GetProperties(GetType());
            foreach (var pair in properties)
            {
                if (!propSet.TryGetValue(pair.Key, Namespaces, out var prop))
                {
                    continue;
                }

                if (!ValueParser.TryParse(pair.Value, prop, out var setter))
                {
                    continue;
                }

                if (onlyLiterals && !(setter is LiteralSetter))
                {
                    continue;
                }

                setter.Set(this);
            }

            if (includeChildren)
            {
                children.ForEach(c => c.BindProperties(includeChildren));
            }
        }

        internal void BindEvents(FrameworkElement root, bool includeChildren = true)
        {
            var elementType = GetType();
            var rootType = root.GetType();

            foreach (var pair in events)
            {
                var eventName = pair.Key;
                var bindingName = pair.Value;

                if (!EventManager.TryGetRoutedEvent(eventName, elementType, Namespaces, out var routedEvent))
                {
                    throw new Exception($"Failed to find event {eventName} on {elementType.Name}");
                }

                var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var rootMethodInfo = rootType.GetMethod(bindingName, flags);
                if (rootMethodInfo == null)
                {
                    throw new Exception($"Failed to find method {rootMethodInfo} on {root.Name} when binding to event {eventName}");
                }

                var handler = (RoutedEventHandler)Delegate.CreateDelegate(typeof(RoutedEventHandler), root, rootMethodInfo);
                eventHandlers.AddHandler(routedEvent, handler);
            }

            if (includeChildren)
            {
                foreach (var child in children)
                {
                    child.BindEvents(root, includeChildren);
                }
            }
        }

        internal void SetNamespaces(IEnumerable<string> namespaces)
        {
            // TODO: serializable hashset?
            foreach (var ns in namespaces)
            {
                if (!this.namespaces.Contains(ns))
                {
                    this.namespaces.Add(ns);
                }
            }
        }

        internal void SetProperties(IEnumerable<Internal.Attribute> attribs)
        {
            foreach (var pair in attribs)
            {
                properties[pair.Key] = pair.Value;
            }

            BindProperties(includeChildren: false, onlyLiterals: true);
        }

        internal void SetEvents(IEnumerable<Internal.Attribute> attribs)
        {
            foreach (var pair in attribs)
            {
                events[pair.Key] = pair.Value;
            }
        }

        internal IReadOnlyDictionary<string, string> GetProperties(bool onlyChanged = true)
        {
            var properties = new Dictionary<string, string>();

            var propSet = ElementRegistry.GetProperties(GetType());
            foreach (var pair in propSet)
            {
                var prop = pair.Value;
                var value = prop.GetValue(this);
                if (!onlyChanged || !IsDefaultValue(value, prop.PropertyType))
                {
                    properties[pair.Key] = value.ToString();
                }
            }

            return properties;
        }

        internal IReadOnlyDictionary<string, string> GetEvents(bool onlyChanged = true)
        {
            return events;
        }

        // TODO have way to specify per element field what default values are (e.g. scale has a default of (1,1,1) not (0,0,0))
        private bool IsDefaultValue(object v, Type type)
        {
            return v == (type.IsValueType ? Activator.CreateInstance(type) : null);
        }

        internal IEnumerable<FrameworkElement> ElementChain
        {
            get
            {
                yield return this;

                var e = parent;
                while (e != null)
                {
                    yield return e;
                    e = e.parent;
                }
            }
        }

        internal IEnumerable<string> Namespaces => ElementChain.SelectMany(e => e.namespaces);
        #endregion Initialization
    }
}