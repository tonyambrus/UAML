using UnityEngine;

namespace Uaml.Core
{
    [CreateAssetMenu(fileName = "Element", menuName = "UAML/Create Schema Element", order = 1)]
    public class Element : ScriptableObject
    {
        public new string name;
        public Component prefab;
        public string className;
        public string containerPath;
        public bool isRoot;
    }
}
