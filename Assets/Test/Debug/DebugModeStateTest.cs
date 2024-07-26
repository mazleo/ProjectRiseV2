using NUnit.Framework;
using ProjectRise.Debug;
using ProjectRise.Debug.External;

namespace ProjectRise.Test.Debug
{
    /// <summary>
    /// Test cases for DebugModeState.
    /// </summary>
    public class DebugModeStateTest
    {
        private DebugModeState _debugModeState;

        [SetUp]
        public void Setup()
        {
            _debugModeState = new DebugModeState();
        }

        [Test]
        public void RegisterMode()
        {
            DebugMode sampleDebugMode = new DebugMode("id", "name", true);
            Assert.That(_debugModeState.GetAll().Count, Is.EqualTo(0));

            _debugModeState.RegisterMode(sampleDebugMode);

            Assert.That(_debugModeState.GetAll().Count, Is.EqualTo(1));
            Assert.That(_debugModeState.ContainsMode(sampleDebugMode.Id), Is.True);
        }

        [Test]
        public void DeregisterAll()
        {
            _debugModeState.RegisterMode(new DebugMode("id1", "name1", true));
            _debugModeState.RegisterMode(new DebugMode("id2", "name2", false));
            Assert.That(_debugModeState.GetAll().Count, Is.EqualTo(2));

            _debugModeState.DeregisterAll();

            Assert.That(_debugModeState.GetAll().Count, Is.EqualTo(0));
        }

        [Test]
        public void GetAll()
        {
            Assert.That(_debugModeState.GetAll().Count, Is.EqualTo(0));
            _debugModeState.RegisterMode(new DebugMode("id1", "name1", true));
            _debugModeState.RegisterMode(new DebugMode("id2", "name2", false));
            _debugModeState.RegisterMode(new DebugMode("id3", "name3", false));

            Assert.That(_debugModeState.GetAll().Count, Is.EqualTo(3));
        }

        [Test]
        public void ContainsMode()
        {
            string id1_true = "id1";
            string id2_false = "id2";
            string id3_true = "id3";

            _debugModeState.RegisterMode(new DebugMode(id1_true, "name1", true));
            _debugModeState.RegisterMode(new DebugMode(id3_true, "name3", false));

            Assert.That(_debugModeState.ContainsMode(id1_true), Is.True);
            Assert.That(_debugModeState.ContainsMode(id2_false), Is.False);
            Assert.That(_debugModeState.ContainsMode(id3_true), Is.True);
        }
    }
}
