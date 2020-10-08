using UnityEngine;

namespace Tweakable.Tests
{
    [Tweakable]
    public class MaterialSetterScript : MonoBehaviour
    {
        public enum EColor
        {
            Red,
            Green,
            Blue
        }

        [Tweak(nameof(ApplyColor))] public EColor color;

        public void ApplyColor()
        {
            var c = Color.white;
            switch (color)
            {
                case EColor.Red: c = Color.red; break;
                case EColor.Green: c = Color.green; break;
                case EColor.Blue: c = Color.blue; break;
            }

            GetComponent<Renderer>().material.color = c;
        }

        [TweakButton("Static Click")]
        public static void StaticClick()
        {
            Debug.Log("Static Click");
        }
    }
}