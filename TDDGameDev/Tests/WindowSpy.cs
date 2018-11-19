using App;
using OpenTK;
using OpenTK.Platform;

namespace Tests
{
    public class WindowSpy : Window
    {
        public WindowSpy(IGameWindow gameWindow, GlClear glClear, Renderer renderer) : base(gameWindow, glClear, renderer)
        {
        }

        public bool RenderCalled { get; private set; }

        public override void Render(object sender, FrameEventArgs e)
        {
            RenderCalled = true;
            base.Render(sender, e);
        }
    }
}