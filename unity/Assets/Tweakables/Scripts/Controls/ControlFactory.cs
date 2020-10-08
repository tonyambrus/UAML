using System.Reflection;
using UnityEngine;

namespace Tweakable
{
    public abstract class ControlFactory
    {
        public abstract RectTransform Create(ControlTemplates templates, object instance, FieldData field);
    }
}