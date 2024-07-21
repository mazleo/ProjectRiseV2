using System;
using NUnit.Framework;
using ProjectRise.World.External;

namespace ProjectRise.Test.World
{
    /// <summary>
    /// Test cases for the GameWorldModel 
    /// </summary>
    public class GameWorldTest
    {
        [Test]
        public void GameWorldModel_DefaultValues()
        {
            GameWorldModel gameWorldModel = new GameWorldModel();
            
            Assert.That(gameWorldModel.Width, Is.EqualTo(GameWorldModel.DefaultWidth));
            Assert.That(gameWorldModel.Length, Is.EqualTo(GameWorldModel.DefaultLength));
            Assert.That(gameWorldModel.Height, Is.EqualTo(GameWorldModel.DefaultHeight));
            Assert.That(gameWorldModel.WaterLevel, Is.EqualTo(GameWorldModel.DefaultWaterLevel));
        }

        [Test]
        public void GameWorldModel_ExpectedValues()
        {
            float expectedWidth = 950F;
            float expectedLength = 670F;
            float expectedHeight = 425F;
            float expectedWaterLevel = 100F;

            GameWorldModel gameWorldModel = new GameWorldModel(expectedWidth, expectedLength, expectedHeight, expectedWaterLevel);
            
            Assert.That(gameWorldModel.Width, Is.EqualTo(expectedWidth));
            Assert.That(gameWorldModel.Length, Is.EqualTo(expectedLength));
            Assert.That(gameWorldModel.Height, Is.EqualTo(expectedHeight));
            Assert.That(gameWorldModel.WaterLevel, Is.EqualTo(expectedWaterLevel));
        }

        [Test]
        public void GameWorldModel_InvalidValues_Throws()
        {
            Assert.Throws<ArgumentException>(() => new GameWorldModel(/*width=*/0, /*length=*/234F, /*height=*/384F, /*waterLevel=*/200F));
            Assert.Throws<ArgumentException>(() => new GameWorldModel(/*width=*/-5F, /*length=*/234F, /*height=*/384F, /*waterLevel=*/200F));
            
            Assert.Throws<ArgumentException>(() => new GameWorldModel(/*width=*/156F, /*length=*/0, /*height=*/384F, /*waterLevel=*/200F));
            Assert.Throws<ArgumentException>(() => new GameWorldModel(/*width=*/156F, /*length=*/-23F, /*height=*/384F, /*waterLevel=*/200F));
            
            Assert.Throws<ArgumentException>(() => new GameWorldModel(/*width=*/156F, /*length=*/100F, /*height=*/0, /*waterLevel=*/200F));
            Assert.Throws<ArgumentException>(() => new GameWorldModel(/*width=*/156F, /*length=*/100F, /*height=*/-75F, /*waterLevel=*/200F));
            
            Assert.Throws<ArgumentException>(() => new GameWorldModel(/*width=*/156F, /*length=*/100F, /*height=*/24F, /*waterLevel=*/0));
            Assert.Throws<ArgumentException>(() => new GameWorldModel(/*width=*/156F, /*length=*/100F, /*height=*/24F, /*waterLevel=*/-1F));
            
            Assert.Throws<ArgumentException>(() => new GameWorldModel(/*width=*/156F, /*length=*/100F, /*height=*/24F, /*waterLevel=*/200F));
        }
    }
}