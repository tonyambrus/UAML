using UnityEngine;

namespace Tweakable
{
    public static class RectTransformExtensions
    {
        public static void MatchParent(this RectTransform rt)
        {
            rt.pivot = Vector2.zero;
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
        }

        public static void SetAnchors(this RectTransform rt, float left, float top, float right, float bottom)
        {
            rt.anchorMin = new Vector2(left, top);
            rt.anchorMax = new Vector2(right, bottom);
        }
    }
}