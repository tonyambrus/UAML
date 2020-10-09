using System;
using System.Collections.Generic;
using Uaml.UX;
using UnityEngine;

namespace Uaml.Core
{
    [CreateAssetMenu(fileName = "Schema", menuName = "UAML/Create Schema", order = 1)]
    public class Schema : ScriptableObject
    {
        public string namespaces;
        public string elementClass;
        public List<GameObject> schema = new List<GameObject>();
        private Dictionary<string, GameObject> dictionary = null;

        private void LoadSchema()
        {
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, GameObject>(StringComparer.OrdinalIgnoreCase);
                foreach (var e in schema)
                {
                    dictionary[e.name] = e;
                }
            }
        }

        public bool TryGetElementPrefab(string name, out GameObject prefab)
        {
            LoadSchema();

            return dictionary.TryGetValue(name, out prefab);
        }

        public GameObject GetElementPrefab(string name) => TryGetElementPrefab(name, out var component) ? component : null;

        internal Transform GetContainerForInstance(string name, Component component)
        {
            var se = component.GetComponent<ShadowElement>();
            var e = se.element;

            if (e.IsRoot)
            {
                return component.transform;
            }

            if (string.IsNullOrEmpty(e.ContainerPath))
            {
                return null;
            }

            return component.transform.Find(e.ContainerPath);

            throw new NotImplementedException("Expect instance to have a ShadowElement with a valid FrameworkElement");
        }
    }
}
