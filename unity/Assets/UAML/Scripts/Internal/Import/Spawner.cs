using System;
using System.Collections.Generic;
using Uaml.Core;
using Uaml.UX;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uaml.Internal
{
    public static class Spawner
    {
        public static GameObject Spawn(Data.Document data, GameObject root = null, bool instantiate = false)
        {
            if (!root)
            {
                root = new GameObject();
            }

            var gos = new List<GameObject>();

            try
            {
                var queue = new Queue<(FrameworkElement parent, GameObject go, Data.Element template)>();
                queue.Enqueue((null, root, data.root));

                while (queue.Count > 0)
                {
                    var (parentElement, go, template) = queue.Dequeue();

                    var element = (FrameworkElement)go.AddComponent(template.type);
                    gos.Add(go);

                    element.gameObject.name = template.name;
                    element.elementName = template.name;
                    element.schema = data.schema;
                    element.SetNamespaces(template.namespaces);
                    element.SetProperties(template.properties.Values);
                    element.SetEvents(template.events.Values);

                    foreach (var childTemplate in template.children)
                    {
                        var childGo = new GameObject(childTemplate.name);
                        gos.Add(childGo);

                        queue.Enqueue((element, childGo, childTemplate));
                    }

                    if (parentElement)
                    {
                        element.SetParent(parentElement);
                    }
                }

                root.GetComponent<FrameworkElement>().ShowSelf = false;
            }
            catch(Exception)
            {
                foreach (var c in gos)
                {
                    GameObject.DestroyImmediate(c.gameObject);
                }

                throw;
            }

            return root;
        }

        //private static FrameworkElement CreateElement(GameObject go, Data.Element template, Scene scene, Schema schema, List<GameObject> gos, bool instantiate = false)
        //{
        //    var element = (FrameworkElement)go.AddComponent(template.type);
        //    gos.Add(go);

        //    element.gameObject.name = template.name;
        //    element.elementName = template.name;
        //    element.schema = schema;
        //    element.SetNamespaces(template.namespaces);
        //    element.SetProperties(template.properties.Values);
        //    element.SetEvents(template.events.Values);

        //    CreateChildElements(element, template, scene, schema, gos, instantiate);

        //    return element;
        //}

        //private static void CreateChildElements(FrameworkElement parent, Data.Element parentTemplate, Scene scene, Schema schema, List<GameObject> gos, bool instantiate = false)
        //{
        //    foreach (var childTemplate in parentTemplate.children)
        //    {
        //        var go = new GameObject(childTemplate.name);
        //        gos.Add(go);

        //        var element = CreateElement(go, childTemplate, scene, schema, gos, instantiate);
        //        parent.AddChild(element);
        //    }
        //}
    }
}
