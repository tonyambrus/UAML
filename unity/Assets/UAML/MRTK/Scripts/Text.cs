using Uaml.Core;
using Uaml.Events;
using Uaml.UX;
using UnityEngine;

namespace Uaml.MRTK
{
    public class Text : Element
    {
        #region Properties
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register<Text, Color>("Color", t => ref t.color);
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register<Text, string>("Value", t => ref t.value);

        [SerializeField] private Color color;
        [SerializeField] private string value;

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        #endregion Properties

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            switch (e.Property.Name)
            {
                case "Color": _Text.SetValue(t => t.color = (Color)e.NewValue); break;
                case "Value": _Text.SetValue(t => t.text = (string)e.NewValue); break;
            }
        }

        private UnityEngine.UI.Text _Text => Instance.GetPath<UnityEngine.UI.Text>("Text");
    }
}