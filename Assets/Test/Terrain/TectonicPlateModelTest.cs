using System;
using NUnit.Framework;
using ProjectRise.ProceduralGeneration.External;
using ProjectRise.Terrain;
using ProjectRise.Terrain.Transformer.TectonicPlate;
using ProjectRise.World.External;

namespace ProjectRise.Test.Terrain
{
    /// <summary>
    /// Test cases for the TectonicPlateModel.
    /// </summary>
    public class TectonicPlateModelTest
    {
        private PerlinModel _perlinModel;
        private GameWorldModel _gameWorldModel;
        private TerrainModel _terrainModel;

        [SetUp]
        public void Setup()
        {
            _perlinModel = PerlinModel.GetBuilder().Lacunarity(0.3F).Build();
            _gameWorldModel = new GameWorldModel();
            _terrainModel = TerrainModel.GetBuilder(_gameWorldModel).Build();
        }

        /// <summary>
        /// Tests the constructor with valid inputs.
        /// </summary>
        [Test]
        public void Constructor_ValidInputs()
        {
            TectonicPlateModel tectonicPlateModel = new TectonicPlateModel(_perlinModel, _terrainModel);
            
            Assert.That(tectonicPlateModel.Model, Is.EqualTo(_perlinModel));
            Assert.That(tectonicPlateModel.TerrainModel, Is.EqualTo(_terrainModel));
        }

        /// <summary>
        /// Tests the constructor with a null perlin model.
        /// </summary>
        [Test]
        public void Constructor_NullPerlinModel()
        {
            Assert.Throws<ArgumentException>(() => new TectonicPlateModel( /*model=*/null, _terrainModel));
        }

        /// <summary>
        /// Tests the constructor with a null terrain model.
        /// </summary>
        [Test]
        public void Constructor_NullTerrainModel()
        {
            Assert.Throws<ArgumentException>(() => new TectonicPlateModel(_perlinModel, /*terrainModel=*/null));
        }

        /// <summary>
        /// Tests the tectonic plate model generation.
        /// </summary>
        [Test]
        public void Generate()
        {
            TectonicPlateModel tectonicPlateModel = new TectonicPlateModel(_perlinModel, _terrainModel);
            
            Assert.That(tectonicPlateModel.Plates.Length, Is.GreaterThan(0));
            AssertValidPlateMap(tectonicPlateModel.PlateMap, tectonicPlateModel.Plates);
        }

        private void AssertValidPlateMap(int[] plateMap, TectonicPlate[] plates)
        {
            int minId = 0;
            int maxId = plates.Length - 1;
            for (int p = 0; p < plateMap.Length; p++)
                Assert.That(plateMap[p], Is.InRange(minId, maxId));
        }
    }
}