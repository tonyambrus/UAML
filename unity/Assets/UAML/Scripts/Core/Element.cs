using System;
using UnityEngine;

namespace Uaml.Core
{
    [Serializable]
    public class Element
    {
        public string name;
        public Component prefab;
        public string className;
        public string containerPath;
        public bool isRoot;
    }

    //[CreateAssetMenu(fileName = "Element", menuName = "UAML/Create Schema Element", order = 1)]
    //public class Element : ScriptableObject
    //{
    //    public Element element;

    //    public new string name;
    //    public Component prefab;
    //    public string className;
    //    public string containerPath;
    //    public bool isRoot;
    //}
}
