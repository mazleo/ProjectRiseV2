using ProjectRise.World.External;
using Zenject;

namespace ProjectRise.Terrain.External
{
    /// <summary>
    /// The terrain system facade.
    /// </summary>
    public class TerrainSystem : IInitializable
    {
        internal TerrainModelManager TerrainModelManager;

        internal GameWorldModel GameWorldModel;

        [Inject]
        TerrainSystem(GameWorldModel gameWorldModel)
        {
            GameWorldModel = gameWorldModel;
        }

        [Inject]
        public void Initialize()
        {
            TerrainModelManager = new TerrainModelManager();
            TerrainModelManager.Initialize(GameWorldModel);
        }
    }
}
