using ProjectRise.Debug.External;
using ProjectRise.World.External;
using Zenject;

namespace ProjectRise.Core
{
    // Core installer for the game.
    public class ProjectRiseInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameWorldModel>().FromNew().AsSingle();

            // Game Systems
            Container
                .Bind<DebugSystem>()
                .FromSubContainerResolve()
                .ByInstaller<DebugSystemInstaller>()
                .AsCached();
        }
    }
}
