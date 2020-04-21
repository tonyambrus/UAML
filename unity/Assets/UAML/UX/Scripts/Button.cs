using System;
using Uaml.Core;
using Uaml.Events;
using UnityEngine;

namespace Uaml.UX
{
    public class Button : Element
    {
        #region Properties
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register<Button, Color>("Color", b => ref b.color);
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register<Button, string>("Text", b => ref b.text);
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Button));

        [SerializeField] private Color color;
        [SerializeField] private string text;

        public Color Color
        {
            get => GetValue<Color>(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public string Text
        {
            get => GetValue<string>(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }
        #endregion Properties

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            switch (e.Property.Name)
            {
                case "Color": _Text.SetValue(t => t.color = (Color)e.NewValue); break;
                case "Text": _Text.SetValue(t => t.text = (string)e.NewValue); break;
            }
        }

        // TODO: automatically generate/reflect from schema to do this
        private UnityEngine.UI.Button _Button => Instance.GetPath<UnityEngine.UI.Button>("Button");
        private UnityEngine.UI.Text _Text => Instance.GetPath<UnityEngine.UI.Text>("Button/Text");
        public void OnEnable() => BindEvent(_Button.onClick, ClickEvent);
        public void OnDisable() => UnbindEvent(_Button.onClick, ClickEvent);
    }
}