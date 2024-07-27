namespace ProjectRise.Terrain
{
    /// <summary>
    /// Interface which transforms the TerrainModel.
    /// </summary>
    /// <typeparam name="TA">Arguments needed by the transformer to execute transformations.</typeparam>
    internal interface ITerrainModelTransformer<TA>
    {
        /// <summary>
        /// Initializes the transformer with the model.
        /// </summary>
        /// <param name="terrainModel">The TerrainModel.</param>
        internal void Initialize(TerrainModel terrainModel);

        /// <summary>
        /// Transforms the TerrainModel.
        /// </summary>
        /// <param name="args">Arguments needed to transform the model.</param>
        internal void Transform(TA args);
    }
}
