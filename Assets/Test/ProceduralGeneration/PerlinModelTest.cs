using System;
using NUnit.Framework;
using ProjectRise.ProceduralGeneration.External;

namespace ProjectRise.Test.ProceduralGeneration
{
    /// <summary>
    /// Test cases for the Perlin Noise Model.
    /// </summary>
    public class PerlinModelTest
    {
        private const float DefaultLacunarity = 1F;
        private const float DefaultPersistence = 1F;
        private const float DefaultXOffset = 0;
        private const float DefaultZOffset = 0;

        [Test]
        public void PerlinModelBuilder_DefaultValues()
        {
            PerlinModel model = PerlinModel.GetBuilder().Build();

            Assert.That(model.Lacunarity, Is.EqualTo(DefaultLacunarity));
            Assert.That(model.Persistence, Is.EqualTo(DefaultPersistence));
            Assert.That(model.XOffset, Is.EqualTo(DefaultXOffset));
            Assert.That(model.ZOffset, Is.EqualTo(DefaultZOffset));
        }

        [Test]
        public void PerlinModelBuilder_SetValues_AreEqual()
        {
            float expectedLacunarity = 3.18F;
            float expectedPersistence = 256F;
            float expectedXOffset = 9992;
            float expectedZOffset = 6348;

            PerlinModel model = PerlinModel
                .GetBuilder()
                .Lacunarity(expectedLacunarity)
                .Persistence(expectedPersistence)
                .XOffset(expectedXOffset)
                .ZOffset(expectedZOffset)
                .Build();

            Assert.That(model.Lacunarity, Is.EqualTo(expectedLacunarity));
            Assert.That(model.Persistence, Is.EqualTo(expectedPersistence));
            Assert.That(model.XOffset, Is.EqualTo(expectedXOffset));
            Assert.That(model.ZOffset, Is.EqualTo(expectedZOffset));
        }

        [Test]
        public void PerlinModelBuilder_SetInvalid_Throws()
        {
            PerlinModel.Builder modelBuilder = PerlinModel.GetBuilder();

            Assert.Throws<ArgumentException>(() => modelBuilder.Lacunarity(-1));
            Assert.Throws<ArgumentException>(() => modelBuilder.Persistence(-1));
            Assert.Throws<ArgumentException>(() => modelBuilder.XOffset(-1));
            Assert.Throws<ArgumentException>(() => modelBuilder.ZOffset(-1));
        }
    }
}
