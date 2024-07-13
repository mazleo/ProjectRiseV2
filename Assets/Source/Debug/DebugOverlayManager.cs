using System.Collections.Generic;
using ProjectRise.Debug.External;
using UnityEngine;
using Zenject;

namespace ProjectRise.Debug
{
    /// <summary>
    /// Manager for the debug overlay.
    /// </summary>
    internal class DebugOverlayManager
    {
        internal Dictionary<string, IDebugUiFactory> UiFactories =
            new Dictionary<string, IDebugUiFactory>();

        private DebugModeState _debugModeState;

        [Inject]
        internal DebugOverlayManager(DebugModeState debugModeState)
        {
            _debugModeState = debugModeState;
        }

        /// <summary>
        /// Registers a mode.
        /// </summary>
        /// <param name="debugMode">The debug mode.</param>
        /// <param name="uiFactory">The UI element factory for the mode.</param>
        internal void RegisterMode(DebugMode debugMode, IDebugUiFactory uiFactory)
        {
            _debugModeState.RegisterMode(debugMode);
            UiFactories.Add(debugMode.Id, uiFactory);
        }

        /// <summary>
        /// Gets all the root UI elements from the factories.
        /// </summary>
        /// <returns></returns>
        internal List<GameObject> GetAllRootUiElements()
        {
            List<GameObject> rootUiElements = new List<GameObject>();
            foreach (IDebugUiFactory uiFactory in UiFactories.Values)
            {
                rootUiElements.AddRange(uiFactory.Create());
            }

            return rootUiElements;
        }

        /// <summary>
        /// Returns whether the mode in question has been registered.
        /// Mostly used for testing.
        /// </summary>
        /// <param name="id">The target Id.</param>
        /// <returns>True if the debug mode has been registered; false otherwise.</returns>
        internal bool ContainsMode(string id)
        {
            return _debugModeState.ContainsMode(id) && UiFactories.ContainsKey(id);
        }
    }
}
