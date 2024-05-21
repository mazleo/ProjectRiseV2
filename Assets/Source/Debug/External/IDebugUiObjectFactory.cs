using System;
using UnityEngine;

namespace ProjectRise.Debug.External
{
    /** Factory that creates UI elements from models. */
    public interface IDebugUiObjectFactory
    {
        internal GameObject Create();

        internal String GetUiObjectFactoryType();

        internal void Update(GameObject uiElementRoot);
    }
}
