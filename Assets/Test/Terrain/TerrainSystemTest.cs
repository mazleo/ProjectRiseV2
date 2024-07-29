using NUnit.Framework;
using ProjectRise.World.External;
using ProjectRise.Terrain.External;
using Zenject;

namespace ProjectRise.Test.Terrain
{
    /// <summary>
    /// Test cases for the TerrainSystem.
    /// </summary>
    [TestFixture]
    public class TerrainSystemTest : ZenjectUnitTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.Bind<GameWorldModel>().FromNew().AsSingle();
            Container.Bind<TerrainSystem>().FromNew().AsSingle();
            Container.Inject(this);
        }

        [Inject]
        private TerrainSystem _terrainSystem;

        [Test]
        public void Initialize()
        {
            Assert.That(_terrainSystem, !Is.Null);
            Assert.That(_terrainSystem.GameWorldModel, !Is.Null);
            Assert.That(_terrainSystem.TerrainModelManager, !Is.Null);
        }
    }
}
