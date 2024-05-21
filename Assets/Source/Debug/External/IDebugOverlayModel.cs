using System;
using ProjectRise.Debug.Mode;

namespace ProjectRise.Debug.External
{
    /** Models containing data for debugging. */
    public interface IDebugOverlayModel
    {
        public String GetKey();

        public String GetHeader();

        public bool HasBeenUpdated();

        internal void SetDebugModeState(DebugModeState state);

        internal String GetUiObjectFactoryType();
    }
}
