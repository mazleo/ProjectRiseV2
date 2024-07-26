using System;
using NUnit.Framework;
using ProjectRise.ProceduralGeneration.External;
using ProjectRise.Terrain;
using ProjectRise.World.External;

namespace ProjectRise.Test.Terrain
{
    public class TerrainModelTest
    {
        [Test]
        public void TerrainModel_DefaultValues()
        {
            GameWorldModel gameWorldModel = new GameWorldModel();

            TerrainModel terrainModel = TerrainModel.GetBuilder(gameWorldModel).Build();

            Assert.That(
                terrainModel.BaseHeightModel,
                Is.EqualTo(TerrainModel.DefaultBaseHeightModel)
            );
            Assert.That(
                terrainModel.SurfaceHeightModel,
                Is.EqualTo(TerrainModel.DefaultSurfaceHeightModel)
            );
            Assert.That(terrainModel.TileSize, Is.EqualTo(TerrainModel.DefaultTileSize));
            Assert.That(
                terrainModel.HorizontalTiles,
                Is.EqualTo(TerrainModel.DefaultHorizontalTiles)
            );
            Assert.That(terrainModel.VerticalTiles, Is.EqualTo(TerrainModel.DefaultVerticalTiles));
        }

        [Test]
        public void TerrainModel_ExpectedValues()
        {
            GameWorldModel gameWorldModel = new GameWorldModel();
            float expectedTileSize = 0.2F;
            int expectedHorizontalTiles = (int)
                Math.Ceiling(gameWorldModel.Width / expectedTileSize);
            int expectedVerticalTiles = (int)Math.Ceiling(gameWorldModel.Length / expectedTileSize);
            if (expectedTileSize * expectedHorizontalTiles > gameWorldModel.Width)
                expectedHorizontalTiles--;
            if (expectedTileSize * expectedVerticalTiles > gameWorldModel.Length)
                expectedVerticalTiles--;
            float[] expectedBaseHeightModel = new float[
                expectedHorizontalTiles * expectedVerticalTiles
            ];
            PerlinModel expectedSurfaceHeightModel = PerlinModel
                .GetBuilder()
                .Lacunarity(1.5F)
                .Persistence(300F)
                .XOffset(1555F)
                .ZOffset(2345F)
                .Build();

            TerrainModel terrainModel = TerrainModel
                .GetBuilder(gameWorldModel)
                .TileSize(expectedTileSize)
                .BaseHeightModel(expectedBaseHeightModel)
                .SurfaceHeightModel(expectedSurfaceHeightModel)
                .Build();

            Assert.That(terrainModel.BaseHeightModel, Is.EqualTo(expectedBaseHeightModel));
            Assert.That(terrainModel.SurfaceHeightModel, Is.EqualTo(expectedSurfaceHeightModel));
            Assert.That(terrainModel.TileSize, Is.EqualTo(expectedTileSize));
            Assert.That(terrainModel.HorizontalTiles, Is.EqualTo(expectedHorizontalTiles));
            Assert.That(terrainModel.VerticalTiles, Is.EqualTo(expectedVerticalTiles));
        }

        [Test]
        public void TerrainModel_InvalidBaseHeightModel_Throw()
        {
            Assert.Throws<ArgumentException>(
                () => TerrainModel.GetBuilder(new GameWorldModel()).BaseHeightModel(new float[3])
            );

            float[] invalidBaseHeightModel = new float[
                (int)GameWorldModel.DefaultWidth * (int)GameWorldModel.DefaultLength
            ];
            invalidBaseHeightModel[233] = -3;
            Assert.Throws<ArgumentException>(
                () =>
                    TerrainModel
                        .GetBuilder(new GameWorldModel())
                        .BaseHeightModel(invalidBaseHeightModel)
            );
        }

        [Test]
        public void TerrainModel_InvalidSurfaceHeightModel_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => TerrainModel.GetBuilder(new GameWorldModel()).SurfaceHeightModel(null)
            );
        }

        [Test]
        public void TerrainModel_InvalidTileSize_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => TerrainModel.GetBuilder(new GameWorldModel()).TileSize(-1)
            );

            Assert.Throws<ArgumentException>(
                () => TerrainModel.GetBuilder(new GameWorldModel()).TileSize(600)
            );
            Assert.Throws<ArgumentException>(
                () => TerrainModel.GetBuilder(new GameWorldModel()).TileSize(800)
            );

            Assert.Throws<ArgumentException>(
                () =>
                    TerrainModel
                        .GetBuilder(new GameWorldModel())
                        .BaseHeightModel(
                            new float[
                                TerrainModel.DefaultHorizontalTiles
                                    * TerrainModel.DefaultVerticalTiles
                            ]
                        )
                        .TileSize(0.0012345F)
            );
        }
    }
}
