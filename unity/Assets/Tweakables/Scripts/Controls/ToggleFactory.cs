using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tweakable
{
    public class ToggleFactory : ControlFactory
    {
        public override RectTransform Create(ControlTemplates templates, object instance, FieldData field)
        {
            var rt = GameObject.Instantiate(templates.toggle).transform as RectTransform;
            var callback = new UnityAction<bool>(value => field.SetValue(instance, value));
            var toggle = rt.GetComponentInChildren<Toggle>();
            toggle.isOn = (bool)field.GetValue(instance);
            toggle.onValueChanged.AddListener(callback);
            return rt;
        }
    }
}