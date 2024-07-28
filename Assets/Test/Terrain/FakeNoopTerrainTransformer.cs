using ProjectRise.Terrain;

namespace ProjectRise.Test.Terrain
{
    /// <summary>
    /// Noop terrain transformer for tests.
    /// </summary>
    internal class FakeNoopTerrainTransformer : ITerrainModelTransformer
    {
        void ITerrainModelTransformer.Initialize(TerrainModel terrainModel) { }

        void ITerrainModelTransformer.Transform(object args) { }
    }
}
