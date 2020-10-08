using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tweakable
{
    public class EnumDropdownFactory : ControlFactory
    {
        public override RectTransform Create(ControlTemplates templates, object instance, FieldData field)
        {
            var rt = GameObject.Instantiate(templates.dropdown).transform as RectTransform;
            var callback = new UnityAction<int>(value => field.SetValue(instance, Enum.Parse(field.FieldType, value.ToString())));
            var dropdown = rt.GetComponentInChildren<Dropdown>();
            dropdown.options = Enum.GetNames(field.FieldType).Select(s => new Dropdown.OptionData(s)).ToList();
            dropdown.value = (int)Convert.ChangeType(field.GetValue(instance), typeof(int));
            dropdown.onValueChanged.AddListener(callback);
            return rt;
        }
    }
}