
using NUnit.Framework;
using ProjectRise.Debug.Mode;

namespace ProjectRise
{
    public class DebugModeStateTest
    {
        [Test]
        public void Enable()
        {
            DebugModeState debugModeState = new DebugModeState();
            
            debugModeState.Enable(DebugMode.ScreenOverlay);
            
            Assert.That(debugModeState.IsEnabled(DebugMode.ScreenOverlay), Is.True);
            Assert.That(debugModeState.IsEnabled(DebugMode.ScreenOverlayFocused), Is.False);
        }
        
        [Test]
        public void IsEnabled()
        {
            DebugModeState debugModeState = new DebugModeState();
            
            debugModeState.Enable(DebugMode.ScreenOverlayFocused);
            
            Assert.That(debugModeState.IsEnabled(DebugMode.ScreenOverlay), Is.False);
            Assert.That(debugModeState.IsEnabled(DebugMode.ScreenOverlayFocused), Is.True);
        }
        
        [Test]
        public void Disable()
        {
            DebugModeState debugModeState = new DebugModeState();
            debugModeState.Enable(DebugMode.ScreenOverlay);
            debugModeState.Enable(DebugMode.ScreenOverlayFocused);
            
            debugModeState.Disable(DebugMode.ScreenOverlay);
            
            Assert.That(debugModeState.IsEnabled(DebugMode.ScreenOverlay), Is.False);
            Assert.That(debugModeState.IsEnabled(DebugMode.ScreenOverlayFocused), Is.True);
        }
    }
}
