using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uaml.Core
{
    public static class Spawn
    {
        public static Component Instantiate(Component prefab, Scene scene = default)
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
