using UnityEngine;
using Zenject;

namespace ProjectRise.Debug.Internal.Interface.Overlay
{
    /** Interface between the scene and the debug system. */
    public class DebugInterface : MonoBehaviour, IInitializable
    {
        [Inject]
        public void Initialize() { }
    }
}
