using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Tweakable
{
    public abstract class TweakableObjectPicker : MonoBehaviour
    {
        public abstract Task<object> PickObjectAsync(CancellationToken cancellationToken);
    }
}