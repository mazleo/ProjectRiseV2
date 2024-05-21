using System.Collections.Generic;

namespace ProjectRise.Debug.Mode
{
    /** Keeps track of enabled debug modes. */
    internal class DebugModeState
    {
        private readonly List<DebugMode> _debugModes = new List<DebugMode>();

        internal DebugModeState() { }

        internal bool IsEnabled(DebugMode mode)
        {
            return _debugModes.Contains(mode);
        }

        internal void Enable(DebugMode mode)
        {
            _debugModes.Add(mode);
        }

        internal void Disable(DebugMode mode)
        {
            _debugModes.Remove(mode);
        }
    }
}
