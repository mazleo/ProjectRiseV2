using Zenject;

namespace ProjectRise.Debug.External
{
    /// <summary>
    /// Facade for the debugging system.
    /// </summary>
    public class DebugSystem
    {
        private DebugOverlayManager _debugOverlayManager;

        [Inject]
        DebugSystem(DebugOverlayManager debugOverlayManager)
        {
            _debugOverlayManager = debugOverlayManager;
        }

        /// <summary>
        /// Exposes the registering of debug modes into the manager.
        /// </summary>
        /// <param name="debugMode">The debug mode.</param>
        /// <param name="uiFactory">The factory for UI elements.</param>
        public void RegisterMode(DebugMode debugMode, IDebugUiFactory uiFactory)
        {
            _debugOverlayManager.RegisterMode(debugMode, uiFactory);
        }
    }
}
