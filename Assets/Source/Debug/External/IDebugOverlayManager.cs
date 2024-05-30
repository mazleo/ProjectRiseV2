using System.Collections.Generic;
using UnityEngine;

namespace ProjectRise.Debug.External
{
    /** Managers the UI Elements and models. */
    public interface IDebugOverlayManager
    {
        public void RegisterModel(IDebugOverlayModel model);

        public void RegisterUiObjectFactory(IDebugUiObjectFactory factory);

        internal List<GameObject> GetAllUiElements();

        internal void CreateAllUiElements();
    }
}
