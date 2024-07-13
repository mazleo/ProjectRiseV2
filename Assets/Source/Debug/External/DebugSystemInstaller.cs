using Zenject;

namespace ProjectRise.Debug.External
{
    /// <summary>
    /// Installer for the Debug System.
    /// </summary>
    public class DebugSystemInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<DebugSystem>().FromNew().AsCached();
            Container.Bind<DebugModeState>().FromNew().AsCached();
            Container.Bind<DebugOverlayManager>().FromNew().AsCached();
        }
    }
}
