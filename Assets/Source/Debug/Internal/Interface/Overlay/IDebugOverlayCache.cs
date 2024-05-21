using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectRise.Debug.Internal.Interface.Overlay
{
    /** Caches the UI Elements. */
    internal interface IDebugOverlayCache
    {
        internal void Cache(String modelKey, GameObject uiElementRoot);

        internal void Get(String modelKey);

        internal List<GameObject> GetAll();

        internal void Remove(String modelKey);
    }
}
