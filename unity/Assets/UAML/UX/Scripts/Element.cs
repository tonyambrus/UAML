using UnityEngine;

namespace Uaml.UX
{
    public class Element : FrameworkElement
    {
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(float), typeof(Element));
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(float), typeof(Element));
        public static readonly DependencyProperty DepthProperty = DependencyProperty.Register("Depth", typeof(float), typeof(Element));
        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(float), typeof(Element));
        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(float), typeof(Element));
        public static readonly DependencyProperty ZProperty = DependencyProperty.Register("Z", typeof(float), typeof(Element));
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Vector3), typeof(Element));
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(Vector3), typeof(Element));

        public float Width
        {
            get => RectTransform.sizeDelta.x;
            set => RectTransform.sizeDelta = new Vector2(value, RectTransform.sizeDelta.y);
        }

        public float Height
        {
            get => RectTransform.sizeDelta.y;
            set => RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, value);
        }

        public float Depth
        {
            get => Scale[2];
            set => SetScale(2, value);
        }

        public float X
        {
            get => Position[0];
            set => SetPosition(0, value);
        }

        public float Y
        {
            get => Position[1];
            set => SetPosition(1, value);
        }

        public float Z
        {
            get => Position[2];
            set => SetPosition(2, value);
        }

        public Vector3 Position
        {
            get => RectTransform.localPosition;
            set => RectTransform.localPosition = value;
        }

        public Vector3 Scale
        {
            get => RectTransform.localScale;
            set => RectTransform.localScale = value;
        }

        private RectTransform RectTransform
        {
            get => (RectTransform)Instance.transform;
        }

        private void SetPosition(int index, float value)
        {
            var p = Position;
            p[index] = value;
            Position = p;
        }

        private void SetScale(int index, float value)
        {
            var s = Scale;
            s[index] = value;
            Scale = s;
        }
    }
}