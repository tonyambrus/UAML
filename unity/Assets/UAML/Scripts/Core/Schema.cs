using System;
using System.Collections.Generic;
using Uaml.UX;
using UnityEngine;

namespace Uaml.Core
{
    [CreateAssetMenu(fileName = "Schema", menuName = "UAML/Create Schema", order = 1)]
    public class Schema : ScriptableObject
    {
        public string elementClass;
        public List<Element> schema = new List<Element>();
        private Dictionary<string, Element> dictionary = null;

        private void LoadSchema()
        {
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, Element>(StringComparer.OrdinalIgnoreCase);
                foreach (var e in schema)
                {
                    dictionary[e.name] = e;
                }
            }
        }

        public bool TryGetElement(string name, out Element element)
        {
            LoadSchema();

            return dictionary.TryGetValue(name, out element);
        }

        public Component GetElementPrefab(string name) => TryGetElementPrefab(name, out var component) ? component : null;

        public bool TryGetElementPrefab(string name, out Component component)
        {
            component = null;
            return TryGetElement(name, out var element) && (component = element.prefab) != null;
        }

        internal Transform GetContainerForInstance(string name, Component component)
        {
            if ((component is ElementBase e) && e.IsRoot)
            {
                return component.transform;
            }

            if (!TryGetElement(name, out var element))
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(element.containerPath))
            {
                return component.transform.Find(element.containerPath);
            }

            return null;
        }
    }
}
