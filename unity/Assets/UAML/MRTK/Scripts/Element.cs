using Uaml.UX;
using UnityEngine;

namespace Uaml.MRTK
{
    public class Element : FrameworkElement
    {
        #region Properties
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register<Element, float>("Width", e => ref e.size.x);
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register<Element, float>("Height", e => ref e.size.y);
        public static readonly DependencyProperty DepthProperty = DependencyProperty.Register<Element, float>("Depth", e => ref e.size.z);
        public static readonly DependencyProperty XProperty = DependencyProperty.Register<Element, float>("X", e => ref e.position.x);
        public static readonly DependencyProperty YProperty = DependencyProperty.Register<Element, float>("Y", e => ref e.position.y);
        public static readonly DependencyProperty ZProperty = DependencyProperty.Register<Element, float>("Z", e => ref e.position.z);

        [SerializeField] private Vector3 size;
        [SerializeField] private Vector3 position;

        public float Width
        {
            get => (float)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        public float Height
        {
            get => (float)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        public float Depth
        {
            get => (float)GetValue(DepthProperty);
            set => SetValue(DepthProperty, value);
        }

        public float X
        {
            get => (float)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public float Y
        {
            get => (float)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public float Z
        {
            get => (float)GetValue(ZProperty);
            set => SetValue(ZProperty, value);
        }
        #endregion Properties

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            var value = e.NewValue;
            switch (e.Property.Name)
            {
                case "Width": RectTransform.sizeDelta = new Vector2((float)value, RectTransform.sizeDelta.y); break;
                case "Height": RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, (float)value); break;
                case "Depth": SetScale(2, (float)value); break;
                case "X": SetPosition(0, (float)value); break;
                case "Y": SetPosition(1, (float)value); break;
                case "Z": SetPosition(2, (float)value); break;
            }
        }

        private RectTransform RectTransform
        {
            get => (RectTransform)Instance.transform;
        }

        private void SetPosition(int index, float value)
        {
            var p = RectTransform.localPosition;
            p[index] = value;
            RectTransform.localPosition = p;
        }

        private void SetScale(int index, float value)
        {
            var s = RectTransform.localScale;
            s[index] = value;
            RectTransform.localScale = s;
        }
    }
}