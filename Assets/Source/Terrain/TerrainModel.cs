using System;
using ProjectRise.ProceduralGeneration.External;
using ProjectRise.World.External;

namespace ProjectRise.Terrain
{
    /// <summary>
    /// Model which contains the data of the terrain.
    /// </summary>
    internal class TerrainModel
    {
        internal static Builder GetBuilder(GameWorldModel gameWorldModel)
        {
            return new Builder(gameWorldModel);
        }

        internal const float DefaultTileSize = 1F;
        internal const int DefaultHorizontalTiles = (int)(
            GameWorldModel.DefaultWidth / DefaultTileSize
        );
        internal const int DefaultVerticalTiles = (int)(
            GameWorldModel.DefaultLength / DefaultTileSize
        );
        internal static readonly float[] DefaultBaseHeightModel = new float[
            (int)DefaultHorizontalTiles * (int)DefaultVerticalTiles
        ];
        internal static readonly PerlinModel DefaultSurfaceHeightModel = PerlinModel
            .GetBuilder()
            .Build();

        /// <summary>
        /// The heights of all tiles.
        /// Expected value is 0 to 1 inclusive.
        /// </summary>
        internal float[] BaseHeightModel;

        /// <summary>
        /// The perlin noise for the tile surfaces.
        /// Each tile will have multiple vertices in which
        /// its heights perlin will influence.
        /// </summary>
        internal PerlinModel SurfaceHeightModel;

        /// <summary>
        /// The size of each tile in the terrain.
        /// </summary>
        internal float TileSize;

        /// <summary>
        /// The number of tiles horizontally (along
        /// the x-axis).
        /// </summary>
        internal int HorizontalTiles;

        /// <summary>
        /// The number of tiles vertically (along
        /// the z-axis).
        /// </summary>
        internal int VerticalTiles;

        private TerrainModel(
            float[] baseHeightModel,
            PerlinModel surfaceHeightModel,
            float tileSize,
            int horizontalTiles,
            int verticalTiles
        )
        {
            BaseHeightModel = baseHeightModel;
            SurfaceHeightModel = surfaceHeightModel;
            TileSize = tileSize;
            HorizontalTiles = horizontalTiles;
            VerticalTiles = verticalTiles;
        }

        internal class Builder
        {
            private float[] _baseHeightModel = DefaultBaseHeightModel;
            private bool _baseHeightModelSet = false;
            private PerlinModel _surfaceHeightModel = DefaultSurfaceHeightModel;
            private float _tileSize = DefaultTileSize;
            private int _horizontalTiles = DefaultHorizontalTiles;
            private int _verticalTiles = DefaultVerticalTiles;
            private readonly GameWorldModel _gameWorldModel;

            internal Builder(GameWorldModel gameWorldModel)
            {
                _gameWorldModel = gameWorldModel;
            }

            internal Builder BaseHeightModel(float[] baseHeightModel)
            {
                TerrainModelValidator.ThrowInvalidBaseHeightModel(
                    baseHeightModel,
                    _horizontalTiles * _verticalTiles
                );
                _baseHeightModel = baseHeightModel;
                _baseHeightModelSet = true;
                return this;
            }

            internal Builder SurfaceHeightModel(PerlinModel surfaceHeightModel)
            {
                TerrainModelValidator.ThrowNullSurfaceHeightModel(surfaceHeightModel);
                _surfaceHeightModel = surfaceHeightModel;
                return this;
            }

            internal Builder TileSize(float tileSize)
            {
                int estimatedHorizontalTiles = (int)Math.Ceiling(_gameWorldModel.Width / tileSize);
                int estimatedVerticalTiles = (int)Math.Ceiling(_gameWorldModel.Length / tileSize);
                if (estimatedHorizontalTiles * tileSize > _gameWorldModel.Width)
                    estimatedHorizontalTiles = estimatedHorizontalTiles - 1;
                if (estimatedVerticalTiles * tileSize > _gameWorldModel.Length)
                    estimatedVerticalTiles = estimatedVerticalTiles - 1;

                if (
                    _baseHeightModelSet
                    && (
                        estimatedHorizontalTiles != _horizontalTiles
                        || estimatedVerticalTiles != _verticalTiles
                    )
                )
                    throw new ArgumentException(
                        "Base height model has been already been set but the horizontal tiles or vertical tiles set by the tile size do not fit the base height model."
                    );

                _tileSize = tileSize;
                _horizontalTiles = estimatedHorizontalTiles;
                _verticalTiles = estimatedVerticalTiles;

                TerrainModelValidator.ThrowInvalidTileSize(
                    _tileSize,
                    _horizontalTiles,
                    _verticalTiles,
                    _gameWorldModel
                );
                return this;
            }

            internal TerrainModel Build()
            {
                return new TerrainModel(
                    _baseHeightModel,
                    _surfaceHeightModel,
                    _tileSize,
                    _horizontalTiles,
                    _verticalTiles
                );
            }
        }
    }
}
