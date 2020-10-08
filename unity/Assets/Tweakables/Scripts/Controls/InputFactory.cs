using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tweakable
{
    public class InputFactory<T> : ControlFactory
    {
        private InputField.ContentType contentType;

        public InputFactory(InputField.ContentType contentType)
        {
            this.contentType = contentType;
        }

        public override RectTransform Create(ControlTemplates templates, object instance, FieldData field)
        {
            var rt = GameObject.Instantiate(templates.input).transform as RectTransform;
            var callback = new UnityAction<string>(value => field.SetValue(instance, Convert.ChangeType(value, typeof(T))));
            var input = rt.GetComponentInChildren<InputField>();
            input.text = Convert.ToString(field.GetValue(instance));
            input.contentType = contentType;
            input.onValueChanged.AddListener(callback);
            return rt;
        }
    }
}