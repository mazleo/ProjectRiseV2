using ProjectRise.Debug.External;
using Zenject;

namespace Source.Core
{
    // Core installer for the game.
    public class PRApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<DebugSystem>()
                .FromSubContainerResolve()
                .ByInstaller<DebugSystemInstaller>()
                .AsCached();
        }
    }
}
