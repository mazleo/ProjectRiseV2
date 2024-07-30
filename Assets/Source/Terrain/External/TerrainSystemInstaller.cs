using Zenject;

namespace ProjectRise.Terrain.External
{
    /// <summary>
    /// Installer for the Terrain System.
    /// </summary>
    public class TerrainSystemInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<TerrainSystem>().FromNew().AsCached();
        }
    }
}
