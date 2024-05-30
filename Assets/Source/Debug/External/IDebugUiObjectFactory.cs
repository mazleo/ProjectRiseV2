using System;
using UnityEngine;

namespace ProjectRise.Debug.External
{
    /** Factory that creates UI elements from models. */
    public interface IDebugUiObjectFactory
    {
        public GameObject Create();

        public String GetUiObjectFactoryType();

        public void Update();
    }
}
