using UnityEngine;

namespace Tweakable.Tests
{
    [Tweakable("Grouped")]
    public class TestScript2 : MonoBehaviour
    {
        public enum State
        {
            First,
            Second,
            Third
        }

        [Tweak("OnBoolChange")] public bool boolValue;
        [Tweak("OnValueChange")] public int intValue;
        [Tweak] public float floatValue;
        [Tweak, Range(0, 1)] public float rangeValue;
        [Tweak] public string stringValue;
        [Tweak] public State state;

        [Tweak] public static int staticIntValue;

        public void OnBoolChange()
        {
            Debug.Log("BoolChanged");
        }

        public void OnValueChange(string propertyName, object prevValue, object newValue)
        {
            Debug.Log("BoolChanged");
        }

        [TweakButton]
        public void Click()
        {
            Debug.Log("Click");
        }

        [TweakButton("Static Click")]
        public static void StaticClick()
        {
            Debug.Log("Static Click");
        }
    }
}