using System;
using NUnit.Framework;
using ProjectRise.Terrain.Transformer.TectonicPlate;

namespace ProjectRise.Test.Terrain
{
    /// <summary>
    /// Test cases for the TectonicPlate objects.
    /// </summary>
    public class TectonicPlateTest
    {
        /// <summary>
        /// Tests the constructor with valid inputs.
        /// </summary>
        [Test]
        public void Constructor_ValidInput()
        {
            TectonicPlate tectonicPlate1 = new TectonicPlate(0, PlateType.Continental);
            TectonicPlate tectonicPlate2 = new TectonicPlate(25, PlateType.Oceanic);
            TectonicPlate tectonicPlate3 = new TectonicPlate(999999, PlateType.Continental);
            
            Assert.That(tectonicPlate1.ID, Is.EqualTo(0));
            Assert.That(tectonicPlate1.Type, Is.EqualTo(PlateType.Continental));
            Assert.That(tectonicPlate2.ID, Is.EqualTo(25));
            Assert.That(tectonicPlate2.Type, Is.EqualTo(PlateType.Oceanic));
            Assert.That(tectonicPlate3.ID, Is.EqualTo(999999));
            Assert.That(tectonicPlate3.Type, Is.EqualTo(PlateType.Continental));
        }

        /// <summary>
        /// Tests the constructor with invalid inputs.
        /// </summary>
        [Test]
        public void Constructor_InvalidInput()
        {
            Assert.Throws<ArgumentException>(() => new TectonicPlate(-7, PlateType.Continental));
        }
    }
}