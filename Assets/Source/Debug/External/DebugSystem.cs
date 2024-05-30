using Zenject;

namespace ProjectRise.Debug.External
{
    /** Facade for the debugging system. */
    public class DebugSystem : IInitializable
    {
        [Inject]
        public DebugSystem() { }

        [Inject]
        public void Initialize() { }
    }
}
