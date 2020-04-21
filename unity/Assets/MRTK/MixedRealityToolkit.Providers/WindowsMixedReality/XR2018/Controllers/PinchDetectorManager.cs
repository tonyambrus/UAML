using UnityEngine;
using UnityEngine.Events;

namespace Microsoft.MixedReality.Toolkit.WindowsMixedReality.Input
{
    public class PinchDetectorManager : MonoBehaviour
    {
        #region Singleton
        private static PinchDetectorManager instance;
        public static PinchDetectorManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PinchDetectorManager>();
                }
                return instance;
            }
        }
        #endregion

        public enum HandPinchModes { System_IsGrasped, System_SelectPressed, Custom_OverwriteSystem, Off }

        public HandPinchModes hands_PinchMode = HandPinchModes.System_SelectPressed;
        public float hands_PinchDist = -1;
        public bool hands_isPinching;
        public UnityEvent OnPinchDown;
        public UnityEvent OnPinchUp;

        private bool wasPinching = false;

        [Header("For Debug")]
        public TextMesh status_PinchMode;
        public TextMesh status_PinchStatus;
        public TextMesh status_PinchDistance;

        public void PinchMode_SelectPressed()
        {
            hands_PinchMode = HandPinchModes.System_SelectPressed;
        }

        public void PinchMode_IsGrasped()
        {
            hands_PinchMode = HandPinchModes.System_IsGrasped;
        }

        public void PinchMode_Custom1()
        {
            hands_PinchMode = HandPinchModes.Custom_OverwriteSystem;
        }

        public void PinchMode_TurnOff()
        {
            hands_PinchMode = HandPinchModes.Off;
        }

        public bool Hands_isPinching
        {
            get { return hands_isPinching; }
            set
            {
                hands_isPinching = value;
            }
        }

        private void DebugText(TextMesh txtMesh, string text)
        {
            if (txtMesh != null)
            {
                txtMesh.text = text;
            }
        }

        private void Update()
        {
            DebugText(status_PinchMode, hands_PinchMode.ToString());
            DebugText(status_PinchStatus, Hands_isPinching.ToString());
            DebugText(status_PinchDistance, (Mathf.Round(hands_PinchDist * 100f) / 100f).ToString());
                        
            if (hands_isPinching && !wasPinching)
            {
                wasPinching = hands_isPinching;
                OnPinchDown.Invoke();
            }
            else if (!hands_isPinching && wasPinching)
            {
                wasPinching = hands_isPinching;
                OnPinchUp.Invoke();
            }
        }
    }
}
