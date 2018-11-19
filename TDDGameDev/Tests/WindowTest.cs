using System;
using App;
using Moq;
using NUnit.Framework;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace Tests
{
    [TestFixture]
    public class WindowTest
    {
        [SetUp]
        public void SetUp()
        {
            _gameWindowMock = new Mock<IGameWindow>();
            _glClear = new Mock<GlClear>();
            _glClear.Setup(m => m.WithClearColor(It.IsAny<Color4>(), It.IsAny<Action>()))
                .Callback<Color4, Action>((color, action) => action());
            _glClear.Setup(m => m.WithCleared(It.IsAny<ClearBufferMask>(), It.IsAny<Action>()))
                .Callback<ClearBufferMask, Action>((clearBufferMask, action) => action());
            _rendererMock = new Mock<Renderer>();
            _window = new Window(_gameWindowMock.Object, _glClear.Object, _rendererMock.Object);
        }

        private Window _window;
        private Mock<IGameWindow> _gameWindowMock;
        private Mock<GlClear> _glClear;
        private Mock<Renderer> _rendererMock;

        [Test]
        public void BindsRenderFunction()
        {
            var window = new WindowSpy(_gameWindowMock.Object, _glClear.Object, _rendererMock.Object);

            _gameWindowMock.Raise(m => m.RenderFrame += null, new FrameEventArgs());

            Assert.IsTrue(window.RenderCalled);
        }

        [Test]
        public void CanBeStarted()
        {
            _window.Start();

            _gameWindowMock.Verify(m => m.Run(20));
        }

        [Test]
        public void ClearsScreenBeforeRendering()
        {
            _glClear.Setup(m => m.WithCleared(It.IsAny<ClearBufferMask>(), It.IsAny<Action>()))
                .Callback<ClearBufferMask, Action>((clearBufferMask, action) =>
                    _gameWindowMock.Verify(m => m.SwapBuffers(), Times.Never)
                );

            _window.Render(this, new FrameEventArgs());

            _glClear.Verify(m => m.WithCleared(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit, It.IsAny<Action>()));
        }

        [Test]
        public void RendersProvidedRenderer()
        {
            _rendererMock.Setup(m => m.Render(It.IsAny<FrameEventArgs>()))
                .Callback<FrameEventArgs>(e =>
                {
                    _glClear.Verify(m => m.WithCleared(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit, It.IsAny<Action>()));
                    _gameWindowMock.Verify(m => m.SwapBuffers(), Times.Never);
                });

            _window.Render(this, new FrameEventArgs());

            _rendererMock.Verify(m => m.Render(It.IsAny<FrameEventArgs>()));
        }

        [Test]
        public void SetsClearColorToMidnightBlue()
        {
            _glClear.Setup(m => m.WithClearColor(It.IsAny<Color4>(), It.IsAny<Action>()))
                .Callback<Color4, Action>((clearColor, action) =>
                    _glClear.Verify(m => m.WithCleared(It.IsAny<ClearBufferMask>(), It.IsAny<Action>()), Times.Never)
                );

            _window.Render(this, new FrameEventArgs());

            _glClear.Verify(m => m.WithClearColor(Color4.MidnightBlue, It.IsAny<Action>()));
        }

        [Test]
        public void SwapsBuffersAfterRendering()
        {
            _window.Render(this, new FrameEventArgs());

            _gameWindowMock.Verify(m => m.SwapBuffers());
        }
    }
}