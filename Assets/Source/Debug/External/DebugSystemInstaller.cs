using ProjectRise.Debug.Internal.Interface.Overlay;
using ProjectRise.Debug.Mode;
using Zenject;

namespace ProjectRise.Debug.External
{
    // Installer for the Debug System
    public class DebugSystemInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<DebugSystem>().FromNew().AsCached();
            Container.Bind<DebugModeState>().FromNew().AsCached();
        }
    }
}
