using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace App
{
    public class Window
    {
        private readonly IGameWindow _gameWindow;
        private readonly GlClear _glClear;
        private readonly Renderer _renderer;

        public Window(IGameWindow gameWindow, GlClear glClear, Renderer renderer)
        {
            _gameWindow = gameWindow;
            _glClear = glClear;
            _renderer = renderer;

            _gameWindow.RenderFrame += Render;
        }

        public virtual void Render(object sender, FrameEventArgs e)
        {
            _glClear.WithClearColor(Color4.MidnightBlue, () =>
                _glClear.WithCleared(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit, () =>
                    _renderer.Render(e)
                )
            );
            _gameWindow.SwapBuffers();
        }

        public void Start()
        {
            _gameWindow.Run(20);
        }
    }
}