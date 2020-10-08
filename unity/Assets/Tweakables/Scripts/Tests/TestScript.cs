using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tweakable.Tests
{
    [Tweakable]
    public class TestScript : MonoBehaviour
    {
        public enum State
        {
            First,
            Second,
            Third
        }

        [Tweak] public bool boolValue;
        [Tweak] public int intValue;
        [Tweak] public float floatValue;
        [Tweak, Range(0, 1)] public float rangeValue;
        [Tweak] public string stringValue;
        [Tweak] public State state;

        [Tweak] public static int staticIntValue;

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