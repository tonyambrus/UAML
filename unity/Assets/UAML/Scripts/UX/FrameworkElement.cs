using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Uaml.Core;
using Uaml.Events;
using Uaml.Internal.Events;
using Uaml.Internal.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Uaml.UX
{
    public class FrameworkElement : DependencyObject //, ISerializationCallbackReceiver
    {
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

        protected virtual void OnDestroy()
        {
            if (instance)
            {
                Destroy(instance);
                instance = null;
            }
        }

        protected virtual void OnParentChanged(FrameworkElement oldParent, FrameworkElement newParent) { }
        protected virtual void OnChildAdded(FrameworkElement child) { }
        protected virtual void OnChildRemoved(FrameworkElement child) { }

        #region Events
        internal EventHandlers eventHandlers = new EventHandlers();

        protected void AddHandler(RoutedEvent routedEvent, RoutedEventHandler handler) => eventHandlers.AddHandler(routedEvent, handler);
        protected void RemoveHandler(RoutedEvent routedEvent, RoutedEventHandler handler) => eventHandlers.RemoveHandler(routedEvent, handler);
        protected void BindEvent(UnityEvent source, RoutedEvent routedEvent) => eventHandlers.BindEvent(this, source, routedEvent);
        protected void UnbindEvent(UnityEvent source, RoutedEvent routedEvent) => eventHandlers.UnbindEvent(source, routedEvent);

        internal void HandleEvent(RoutedEventArgs args) => eventHandlers.HandleEvent(this, args);
        
        #endregion Events

        #region Visibility
        private bool showSelf = true;
        private bool showInHierarchy = true;

        internal bool ShowSelf
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

        internal bool ShowInHierarchy
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

        internal void ForceShow()
        {
            // TODO: fix me
            var p = this;
            while (p)
            {
                p.ShowInHierarchy = true;
                p.ShowSelf = true;
                p = p.Parent;
            }
        }

        private void OnTransformParentChanged() => UpdateParent();

        private void UpdateParent()
        {
            var parentT = transform.parent;
            while (parentT != null)
            {
                var fe = parentT.GetComponent<FrameworkElement>();
                if (fe && fe.Container)
                {
                    SetParent(fe);
                    return;
                }

                parentT = parentT.parent;
            }

            SetParent(null);
        }

        public void SetParent(FrameworkElement newParent, bool suppressNotification = false)
        {
            if (parent != newParent)
            {
                if (parent)
                {
                    parent.RemoveChild(this, suppressNotification: true);
                }

                var oldParent = parent;
                parent = newParent;

                if (parent)
                {
                    parent.AddChild(this, suppressNotification: true);
                }

                if (!suppressNotification && instance)
                {
                    OnParentChanged(oldParent, parent);
                }
            }
        }

        private void UpdateShow()
        {
            children.ForEach(c => { if (c) c.ShowInHierarchy = ShowInHierarchy; });
            if (instance) 
            {
                instance.gameObject.hideFlags = ShowInHierarchy ? HideFlags.None : HideFlags.HideInHierarchy;
            }

#if UNITY_EDITOR
            UnityEditor.EditorApplication.DirtyHierarchyWindowSorting();
#endif
        }
        #endregion Visibility

        #region Initialization
        [SerializeField] private ShadowElement instance;
        [SerializeField] private string instancePath;
        [SerializeField] private Transform container;
        [SerializeField] internal string elementName;
        [SerializeField] internal Schema schema;

        public virtual string ContainerPath => null;

        public string ElementName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(elementName))
                {
                    foreach (var t in Util.GetTypeChain(GetType()))
                    {
                        if (Schema.TryGetElementPrefab(t.Name, out var _))
                        {
                            elementName = t.Name;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(elementName))
                    {
                        throw new Exception($"Type {GetType()} not contained in schema");
                    }
                }

                return elementName;
            }
        }


        protected Schema Schema
        {
            get
            {
                if (!schema)
                {
                    schema = DefaultSchema.Schema;
                }

                return schema;
            }
        }

#if UNITY_EDITOR
        internal Component EditorInstance
        {
            get
            {
                if (UnityEditor.EditorUtility.IsPersistent(gameObject))
                {
                    return null; // original prefab, don't trigger instantiation
                } 

                if (!instance)
                {
                    // instantiate whole hierarchy
                    foreach (var child in Children)
                    {
                        var _ = child.EditorInstance;
                    }
                }

                return Instance;
            }
        }
#endif

        protected Component Instance
        {
            get
            {
                if (instance && instance.element != this)
                {
                    instance = null;
                }

                if (!instance && Schema)
                {
                    var prefab = Schema.GetElementPrefab(ElementName);
                    var go = Spawn.Instantiate(prefab, gameObject.scene);

                    //instance = RootElement.transform.Find(InstancePath)?.GetComponent<ShadowElement>();

                    if (!instance)
                    {
                        instance = go.GetComponent<ShadowElement>();
                    }

                    if (!instance)
                    {
                        instance = go.AddComponent<ShadowElement>();
                    }
                    instance.element = this;

                    UpdateParent();
                    OnInstanceReparented();
                }

                return instance;
            }
        }

        protected Transform Container
        {
            get
            {
                if (!container && Schema)
                {
                    container = Schema.GetContainerForInstance(ElementName, Instance);
                }

                return container;
            }
        }

        internal void OnInstanceReparented()
        {
            var parentContainer = (IsRoot || !Parent?.Container) ? transform : Parent.Container;
            instance.transform.SetParent(parentContainer, false);
            instance.name = "_" + ElementName;
            instancePath = null;
        }

        private void AddChild(FrameworkElement element, bool suppressNotification = false)
        {
            if (instance && !container)
            {
                throw new Exception($"Element {ElementName} can't have children");
            }

            var oldParent = element.parent;

            children.Add(element);
            element.transform.SetParent(transform, false);
            element.ShowInHierarchy = ShowInHierarchy;
            element.ShowSelf = false;

            const int elementPrefix = 0xF000;

            // We must have a unique identifer for path for Unity AssetImportContext to use
            // so just prepend a unique non-printable unicode character.
            var id = elementPrefix + (children.Count - 1);
            element.name = $"{char.ConvertFromUtf32(id)}{element.ElementName}";
            
            if (instance)
            {
                OnChildAdded(element);
            }
        }

        private void RemoveChild(FrameworkElement element, bool suppressNotification = false)
        {
            if (!container)
            {
                throw new Exception($"Element {ElementName} can't have children");
            }

            if (!element)
            {
                throw new NullReferenceException(nameof(element));
            }

            children.Remove(element);
            element.ShowInHierarchy = true;
            element.ShowSelf = false;
            element.SetParent(null, suppressNotification);

            if (instance)
            {
                OnChildRemoved(element);
            }
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

        internal void Init(FrameworkElement root)
        {
            // HACK: unity leaving null elements around, clear out here
            for (var i = children.Count - 1; i >= 0; i--)
            {
                if (!children[i])
                {
                    children.RemoveAt(i);
                }
            }

            BindProperties();
            BindEvents(root);
        }

        internal void BindEvents(FrameworkElement root, bool includeChildren = true)
        {
            var elementType = GetType();
            var rootType = root.GetType();

            foreach (var pair in events)
            {
                var eventName = pair.Key;
                var bindingName = pair.Value;

                if (!EventManager.TryGetEvent(eventName, elementType, Namespaces, out var routedEvent))
                {
                    throw new Exception($"Failed to find event {eventName} on {elementType.Name}");
                }

                var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                var rootMethodInfo = rootType.GetMethod(bindingName, flags);
                if (rootMethodInfo == null)
                {
                    throw new Exception($"Failed to find method {rootMethodInfo} on {root.ElementName} when binding to event {eventName}");
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

            ApplyPropertiesToDependencyObject();
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

                if (onlyChanged)
                {
                    if (Util.IsDefaultValue(value, prop.PropertyType))
                    {
                        continue;
                    }
                }

                properties[pair.Key] = (string)ValueConverter.Convert(value, typeof(string));
            }

            return properties;
        }

        internal IReadOnlyDictionary<string, string> GetEvents(bool onlyChanged = true)
        {
            return events;
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

        //#region Serialization
        //[Serializable]
        //private struct TypedProperty
        //{
        //    public string ownerType;
        //    public string propertyName;
        //    public string value;
        //}

        //void ISerializationCallbackReceiver.OnAfterDeserialize() => ApplyPropertiesToDependencyObject();
        //void ISerializationCallbackReceiver.OnBeforeSerialize() => GetPropertiesFromDependencyObject();

        internal void ApplyPropertiesToDependencyObject(bool includeChildren = false)
        {
            SignalPropertyChanges = false;

            var propSet = ElementRegistry.GetProperties(GetType());
            foreach (var pair in properties)
            {
                if (!propSet.TryGetValue(pair.Key, Namespaces, out var prop))
                {
                    continue;
                }

                var result = ValueConverter.Convert(pair.Value, prop.PropertyType);
                SetValue(prop, result);
            }

            SignalPropertyChanges = true;

            if (includeChildren)
            {
                foreach (var child in Children)
                {
                    child.ApplyPropertiesToDependencyObject();
                }
            }
        }

        //internal void GetPropertiesFromDependencyObject()
        //{
        //    SignalPropertyChanges = false;

        //    var elementInfo = ElementRegistry.GetProperties(GetType());

        //    properties.Clear();
        //    foreach (var p in elementInfo)
        //    {
        //        try
        //        {
        //            var value = GetValue(p.Value);
        //            var strValue = (string)ValueConverter.Convert(value, typeof(string));

        //            //Debug.Log($"serialize: ({p.Value.OwnerType.Name}, {p.Value.Name})='{strValue}'");
        //            properties.Add(p.Value.Name, strValue);
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.LogException(e);
        //        }
        //    }

        //    SignalPropertyChanges = true;
        //}
        //#endregion Serialization
    }
}