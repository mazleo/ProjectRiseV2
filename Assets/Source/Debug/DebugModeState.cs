using ProjectRise.Debug.External;
using System.Collections.Generic;
using Zenject;

namespace ProjectRise.Debug
{
    /// <summary>
    /// Holds all debug modes and provides their states.
    /// </summary>
    internal class DebugModeState
    {
        private readonly List<DebugMode> _debugModes = new List<DebugMode>();

        [Inject]
        internal DebugModeState() { }

        /// <summary>
        /// Gets All the debug modes.
        /// </summary>
        /// <returns>All debug modes.</returns>
        internal List<DebugMode> GetAll()
        {
            return _debugModes;
        }

        /// <summary>
        /// Registers a debug mode to the list.
        /// </summary>
        /// <param name="debugMode">The debug mode to register.</param>
        internal void RegisterMode(DebugMode debugMode)
        {
            _debugModes.Add(debugMode);
        }

        /// <summary>
        /// Clears all registered debug modes.
        /// </summary>
        internal void DeregisterAll()
        {
            _debugModes.Clear();
        }

        /// <summary>
        /// Checks whether any registered DebugMode has the
        /// given Id.
        /// Mostly used for testing.
        /// </summary>
        /// <param name="id">The Id to check.</param>
        /// <returns>True if the target debug mode exists; false otherwise.</returns>
        internal bool ContainsMode(string id)
        {
            foreach (DebugMode debugMode in _debugModes)
            {
                if (debugMode.Id.Equals(id))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
