using Uaml.Core;
using Uaml.Events;
using UnityEngine;

namespace Uaml.UX
{
    public class Text : Element
    {
        private UnityEngine.UI.Text _Text => instance.GetPath<UnityEngine.UI.Text>("Text");

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