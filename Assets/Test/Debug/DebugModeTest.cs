using ProjectRise.Debug.External;
using NUnit.Framework;

namespace ProjectRise.Test.Debug
{
    /// <summary>
    /// Test cases for DebugMode.
    /// </summary>
    public class DebugModeTest
    {
        [Test]
        public void DebugMode()
        {
            string expectedId = "performance--hardware-metrics";
            string expectedName = "Hardware Metrics";
            bool expectedEnabled = true;

            DebugMode debugMode = new DebugMode(expectedId, expectedName, expectedEnabled);

            Assert.That(debugMode.Id, Is.EqualTo(expectedId));
            Assert.That(debugMode.Name, Is.EqualTo(expectedName));
            Assert.That(debugMode.Enabled, Is.EqualTo(expectedEnabled));
        }
    }
}
