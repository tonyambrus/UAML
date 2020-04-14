using Uaml.Core;
using Uaml.Events;
using UnityEngine;

namespace Uaml.UX
{
    public class Button : Element
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(Button));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Button));

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Button));

        public Color Color
        {
            get => _Text.GetValue(t => t.color);
            set => _Text.SetValue(t => t.color = value);
        }

        public string Text
        {
            get => _Text.GetValue(t => t.text);
            set => _Text.SetValue(t => t.text = value);
        }

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        #region Internal
        // TODO: automatically generate/reflect from schema to do this
        private UnityEngine.UI.Button _Button => instance.GetPath<UnityEngine.UI.Button>("Button");
        private UnityEngine.UI.Text _Text => instance.GetPath<UnityEngine.UI.Text>("Button/Text");
        public void OnEnable() => BindEvent(_Button.onClick, ClickEvent);
        public void OnDisable() => UnbindEvent(_Button.onClick, ClickEvent);
        #endregion Internal
    }
}