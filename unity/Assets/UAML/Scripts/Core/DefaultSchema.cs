using System.Linq;
using UnityEngine;

namespace Uaml.Core
{
    public class DefaultSchema : MonoBehaviour
    {
        public Schema schema;

        private static DefaultSchema instance;

        public static Schema Schema
        {
            get
            {
                if (!instance)
                {
                    instance = FindObjectOfType<DefaultSchema>();
                }

                if (!instance)
                {
                    instance = new GameObject("UamlDefaultSchema").AddComponent<DefaultSchema>();
                }

                if (!instance.schema)
                {
                    instance.schema = Resources.FindObjectsOfTypeAll<Schema>().FirstOrDefault();
                }

                return instance.schema;
            }
        }
    }
}
