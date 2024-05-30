using UnityEngine;
using Zenject;

namespace ProjectRise.Debug.Internal.Interface.Overlay
{
    /** Hosts the UI elements for the debug system. */
    public class DebugOverlay : MonoBehaviour, IInitializable
    {
        [Inject]
        public void Initialize() { }
    }
}
