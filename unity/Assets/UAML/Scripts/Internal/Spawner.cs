using System;
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

        private static ElementBase CreateElement(GameObject go, Data.Element template, Scene scene, Schema schema)
        {
            var prefab = schema.GetElementPrefab(template.name);
            var instance = Instantiate(prefab, scene);

            var element = (ElementBase)go.AddComponent(template.type);
            element.gameObject.name = template.name;
            element.Name = template.name;
            element.schema = schema;
            element.SetInstance(instance);
            element.SetProperties(template.properties);
            element.SetEvents(template.events);

            CreateChildElements(element, template, scene, schema);

            return element;
        }

        private static void CreateChildElements(ElementBase parent, Data.Element parentTemplate, Scene scene, Schema schema)
        {
            foreach (var childTemplate in parentTemplate.children)
            {
                var go = new GameObject(childTemplate.name);
                var element = CreateElement(go, childTemplate, scene, schema);
                parent.AddChild(element);
            }
        }

        private static Component Instantiate(Component prefab, Scene scene = default)
        {
            try
            {
                var instance = UnityEngine.Object.Instantiate(prefab);
                if (instance.gameObject.scene != scene && scene != default)
                {
                    SceneManager.MoveGameObjectToScene(instance.gameObject, scene);

                    // park under root
                    instance.transform.SetParent(scene.GetRootGameObjects()[0].transform, false);
                }

                return instance;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to Instantiate prefab {prefab}", e);
            }
        }
    }
}
