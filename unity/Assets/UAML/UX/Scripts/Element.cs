using UnityEngine;

namespace Uaml.UX
{
    public class Element : ElementBase
    {
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
            get => (RectTransform)instance.transform;
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