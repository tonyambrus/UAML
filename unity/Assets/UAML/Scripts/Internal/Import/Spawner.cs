using Uaml.Core;
using Uaml.UX;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uaml.Internal
{
    public static class Spawner
    {
        public static GameObject Spawn(Data.Document data, GameObject go = null)
        {
            if (!go)
            {
                go = new GameObject();
            }

            var document = CreateElement(go, data.root, go.scene, data.schema);
            document.ShowSelf = false;

            return go;
        }

        private static FrameworkElement CreateElement(GameObject go, Data.Element template, Scene scene, Schema schema)
        {
            var element = (FrameworkElement)go.AddComponent(template.type);
            element.gameObject.name = template.name;
            element.name = template.name;
            element.schema = schema;
            element.SetNamespaces(template.namespaces);
            element.SetProperties(template.properties.Values);
            element.SetEvents(template.events.Values);

            CreateChildElements(element, template, scene, schema);

            return element;
        }

        private static void CreateChildElements(FrameworkElement parent, Data.Element parentTemplate, Scene scene, Schema schema)
        {
            foreach (var childTemplate in parentTemplate.children)
            {
                var go = new GameObject(childTemplate.name);
                var element = CreateElement(go, childTemplate, scene, schema);
                parent.AddChild(element);
            }
        }
    }
}
