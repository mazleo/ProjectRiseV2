using ProjectRise.World.External;

namespace ProjectRise.Terrain
{
    /// <summary>
    /// Manager for the terrain model as well as
    /// the transformers.
    /// </summary>
    internal class TerrainModelManager
    {
        private TerrainModel _terrainModel;

        private readonly TerrainTransformerCollection _coreTransformers =
            new TerrainTransformerCollection();
        private readonly TerrainTransformerCollection _extendedTransformers =
            new TerrainTransformerCollection();

        internal void Initialize(GameWorldModel gameWorldModel)
        {
            _terrainModel = TerrainModel.GetBuilder(gameWorldModel).Build();
        }

        internal TerrainModel GetModel()
        {
            return _terrainModel;
        }

        internal ITerrainModelTransformer GetCoreTransformer(string id)
        {
            return _coreTransformers.Get(id);
        }

        internal ITerrainModelTransformer GetExtendedTransformer(string id)
        {
            return _extendedTransformers.Get(id);
        }
    }
}
