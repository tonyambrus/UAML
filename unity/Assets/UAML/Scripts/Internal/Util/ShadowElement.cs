using UnityEngine;

namespace Uaml.UX
{
    public class ShadowElement : MonoBehaviour
    {
        public FrameworkElement element;

        public void OnTransformParentChanged()
        {
            element.OnInstanceReparented();
        }

        private void Awake()
        {
            if (!element)
            {
                Destroy(gameObject);
            }
        }
    }
}