using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Tweakable
{
    public class MousePickerTweakableObjectPicker : TweakableObjectPicker
    {
        public GameObject go;

        public override async Task<object> PickObjectAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                go = null;

                if (Physics.Raycast(ray.origin, ray.direction, out var hitInfo, float.MaxValue))
                {
                    go = hitInfo.collider.gameObject;

                    if (Input.GetMouseButtonDown(0))
                    {
                        return hitInfo.collider.gameObject;
                    }
                }

                await Task.Yield();
            }

            return null;
        }
    }
}