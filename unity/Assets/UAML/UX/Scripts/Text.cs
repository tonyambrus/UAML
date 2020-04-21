using Uaml.Core;
using Uaml.Events;
using UnityEngine;

namespace Uaml.UX
{
    public class Text : Element
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(Text));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(Text));

        private UnityEngine.UI.Text _Text => Instance.GetPath<UnityEngine.UI.Text>("Text");

        public Color Color
        {
            get => _Text.GetValue(t => t.color);
            set => _Text.SetValue(t => t.color = value);
        }

        public string Value
        {
            get => _Text.GetValue(t => t.text);
            set => _Text.SetValue(t => t.text = value);
        }
    }
}