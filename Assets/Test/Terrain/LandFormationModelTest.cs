using System;
using NUnit.Framework;
using ProjectRise.ProceduralGeneration.External;
using ProjectRise.Terrain;
using ProjectRise.Terrain.Transformer.Landform;
using ProjectRise.Terrain.Transformer.TectonicPlate;
using ProjectRise.World.External;

namespace ProjectRise.Test.Terrain
{
    /// <summary>
    /// Test cases for the LandformationModelTest.
    /// </summary>
    public class LandFormationModelTest
    {
        private TectonicPlateModel _tectonicPlateModel;
        private TerrainModel _terrainModel;
        private GameWorldModel _gameWorldModel;
        private PerlinModel _perlinModel;

        [SetUp]
        public void Setup()
        {
            _gameWorldModel = new GameWorldModel();
            _terrainModel = TerrainModel.GetBuilder(_gameWorldModel).Build();
            _perlinModel = PerlinModel.GetBuilder().Lacunarity(0.3F).Build();
            _tectonicPlateModel = new TectonicPlateModel(_perlinModel, _terrainModel);
        }

        [Test]
        public void Constructor_ValidInputs()
        {
            LandFormationModel landFormationModel = new LandFormationModel(
                _tectonicPlateModel,
                _terrainModel,
                _gameWorldModel
            );

            Assert.That(landFormationModel.TectonicPlateModel, Is.EqualTo(_tectonicPlateModel));
            Assert.That(landFormationModel.TerrainModel, Is.EqualTo(_terrainModel));
            Assert.That(landFormationModel.GameWorldModel, Is.EqualTo(_gameWorldModel));
            Assert.That(landFormationModel.DistanceModel, !Is.Null);
            Assert.That(landFormationModel.HeightModel, !Is.Null);
        }

        [Test]
        public void Constructor_NullInputs()
        {
            Assert.Throws<ArgumentException>(
                () =>
                    new LandFormationModel( /*tectonicPlateModel=*/
                        null,
                        _terrainModel,
                        _gameWorldModel
                    )
            );
            Assert.Throws<ArgumentException>(
                () =>
                    new LandFormationModel(
                        _tectonicPlateModel, /*terrainModel=*/
                        null,
                        _gameWorldModel
                    )
            );
            Assert.Throws<ArgumentException>(
                () =>
                    new LandFormationModel(
                        _tectonicPlateModel,
                        _terrainModel, /*gameWorldModel=*/
                        null
                    )
            );
        }
    }
}
