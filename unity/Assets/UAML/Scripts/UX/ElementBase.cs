using System;
using System.Collections.Generic;
using Uaml.Core;
using Uaml.Events;
using Uaml.Internal;
using Uaml.Internal.Reflection;
using UnityEngine;

namespace Uaml.UX
{
    public class ElementBase : MonoBehaviour
    {
        public StringSerializableDictionary properties = new StringSerializableDictionary();
        public StringSerializableDictionary events = new StringSerializableDictionary();
        public List<ElementBase> children = new List<ElementBase>();
        public ElementBase parent;

        public virtual bool IsRoot => false;
        public ElementBase RootElement => parent ? parent.RootElement : Element;
        private ElementBase Element => (ElementBase)this;

        protected virtual void Awake() { }

        #region Events
        internal EventHandlers eventHandlers = new EventHandlers();

        protected void AddHandler(RoutedEvent routedEvent, RoutedEventHandler handler) => eventHandlers.AddHandler(routedEvent, handler);
        protected void RemoveHandler(RoutedEvent routedEvent, RoutedEventHandler handler) => eventHandlers.RemoveHandler(routedEvent, handler);
        protected void BindEvent(UnityEngine.UI.Button.ButtonClickedEvent source, RoutedEvent routedEvent) => eventHandlers.BindEvent(Element, source, routedEvent);
        protected void UnbindEvent(UnityEngine.UI.Button.ButtonClickedEvent source, RoutedEvent routedEvent) => eventHandlers.UnbindEvent(source, routedEvent);

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

        public void AddChild(ElementBase element)
        {
            if (!container)
            {
                throw new Exception($"Element {Name} can't have children");
            }

            children.Add(element);
            element.parent = Element;
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

        internal void SetProperties(Dictionary<string, string> attribs)
        {
            var propSet = ElementRegistry.GetProperties(GetType());
            foreach (var pair in attribs)
            {
                if (propSet.TryGetValue(pair.Key, out var prop))
                {
                    properties[pair.Key] = pair.Value;

                    if (ValueConverter.TryParse(pair.Value, prop.PropertyType, out var v))
                    {
                        prop.SetValue(this, v);
                    }
                }
            }
        }

        internal void SetEvents(Dictionary<string, string> attribs)
        {
            foreach (var pair in attribs)
            {
                if (EventManager.HasRoutedEvent(pair.Key))
                {
                    events[pair.Key] = pair.Value;
                }
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
        #endregion Initialization
    }
}