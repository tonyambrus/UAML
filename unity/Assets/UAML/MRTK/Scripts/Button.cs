using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using Uaml.Core;
using Uaml.Events;
using Uaml.UX;
using UnityEngine;

namespace Uaml.MRTK
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
        private PressableButtonHoloLens2 _Button => Instance.GetPath<PressableButtonHoloLens2>("Button");
        private TextMeshPro _Text => Instance.GetPath<TextMeshPro>("Button/IconAndText/TextMeshPro");
        public void OnEnable() => BindEvent(_Button.ButtonPressed, ClickEvent);
        public void OnDisable() => UnbindEvent(_Button.ButtonPressed, ClickEvent);
    }
}