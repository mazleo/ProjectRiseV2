using System;
using ProjectRise.ProceduralGeneration.External;
using ProjectRise.World.External;

namespace ProjectRise.Terrain
{
    /// <summary>
    /// Validates fields of the TerrainModel.
    /// </summary>
    internal class TerrainModelValidator
    {
        internal static void ThrowInvalidBaseHeightModel(
            float[] providedBaseHeightModel,
            int expectedLength
        )
        {
            if (providedBaseHeightModel.Length != expectedLength)
                throw new ArgumentException("Base height model not of expected length.");
            foreach (float height in providedBaseHeightModel)
            {
                if (height < 0)
                    throw new ArgumentException("Base height model heights cannot be negative.");
                if (height > 1)
                    throw new ArgumentException(
                        "Base height model heights cannot be greater than 1."
                    );
            }
        }

        internal static void ThrowNullSurfaceHeightModel(PerlinModel providedSurfaceHeightModel)
        {
            if (providedSurfaceHeightModel == null)
                throw new ArgumentException("Surface height model cannot be null.");
        }

        internal static void ThrowInvalidTileSize(
            float providedTileSize,
            int horizontalTiles,
            int verticalTiles,
            GameWorldModel gameWorldModel
        )
        {
            if (providedTileSize <= 0)
                throw new ArgumentException("Tile size cannot be non-positive.");
            if (providedTileSize > gameWorldModel.Width || providedTileSize > gameWorldModel.Length)
                throw new ArgumentException("Tile size cannot be larger than the game world.");
            if (providedTileSize * horizontalTiles > gameWorldModel.Width)
                throw new ArgumentException("Tile size does not fit the game world width.");
            if (providedTileSize * verticalTiles > gameWorldModel.Length)
                throw new ArgumentException("Tile size does not fit the game world length.");
        }
    }
}
