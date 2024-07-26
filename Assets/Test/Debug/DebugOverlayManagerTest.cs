using System.Collections.Generic;
using NUnit.Framework;
using ProjectRise.Debug;
using ProjectRise.Debug.External;
using UnityEngine;

namespace ProjectRise.Test.Debug
{
    /// <summary>
    /// Test cases for the DebugOverlayManager.
    /// </summary>
    public class DebugOverlayManagerTest
    {
        private DebugModeState _debugModeState;
        private DebugOverlayManager _debugOverlayManager;

        [SetUp]
        public void Setup()
        {
            _debugModeState = new DebugModeState();
            _debugOverlayManager = new DebugOverlayManager(_debugModeState);
        }

        [Test]
        public void RegisterMode()
        {
            string id = "id";
            Assert.That(_debugOverlayManager.ContainsMode(id), Is.False);

            _debugOverlayManager.RegisterMode(new DebugMode(id, "name", true), new FakeUiFactory());

            Assert.That(_debugOverlayManager.ContainsMode(id), Is.True);
        }

        [Test]
        public void ContainsMode()
        {
            string id1 = "id1";
            string id2 = "id2";
            Assert.That(_debugOverlayManager.ContainsMode(id1), Is.False);
            Assert.That(_debugOverlayManager.ContainsMode(id2), Is.False);

            _debugOverlayManager.RegisterMode(
                new DebugMode(id1, "name1", true),
                new FakeUiFactory()
            );

            Assert.That(_debugOverlayManager.ContainsMode(id1), Is.True);
            Assert.That(_debugOverlayManager.ContainsMode(id2), Is.False);
            Assert.That(_debugModeState.ContainsMode(id1), Is.True);
            Assert.That(_debugModeState.ContainsMode(id2), Is.False);
            Assert.That(_debugOverlayManager.UiFactories.ContainsKey(id1), Is.True);
            Assert.That(_debugOverlayManager.UiFactories.ContainsKey(id2), Is.False);
        }

        [Test]
        public void GetAllRootUiElements()
        {
            FakeUiFactory uiFactory = new FakeUiFactory();
            Assert.That(_debugOverlayManager.GetAllRootUiElements().Count, Is.EqualTo(0));

            _debugOverlayManager.RegisterMode(new DebugMode("id", "name", false), uiFactory);

            Assert.That(
                _debugOverlayManager.GetAllRootUiElements(),
                Is.EqualTo(uiFactory.GameObjects)
            );
        }
    }

    class FakeUiFactory : IDebugUiFactory
    {
        internal List<GameObject> GameObjects = new List<GameObject>();

        internal FakeUiFactory()
        {
            GameObjects.Add(new GameObject());
            GameObjects.Add(new GameObject());
            GameObjects.Add(new GameObject());
        }

        public List<GameObject> Create()
        {
            return GameObjects;
        }
    }
}
