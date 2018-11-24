using System;
using System.Runtime.InteropServices;
using App;
using Moq;
using NUnit.Framework;
using OpenTK.Graphics.OpenGL;

namespace Tests
{
    [TestFixture]
    internal class VboFactoryTest
    {
        [SetUp]
        public void SetUp()
        {
            _vertices = new[] {new FakeVertex()};
            _glBufferMock = new Mock<GlBuffer>();
            _glBufferMock.Setup(m => m.GenerateBuffers(1))
                .Returns(new[] {2});
            _glBufferMock.Setup(m => m.WithBoundBuffer(It.IsAny<BufferTarget>(), It.IsAny<int>(), It.IsAny<Action>()))
                .Callback<BufferTarget, int, Action>((bufferTarget, bufferId, action) => action());
            _glBufferMock.Setup(m => m.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize))
                .Returns(Marshal.SizeOf<FakeVertex>());
            _factory = new VboFactory<FakeVertex>(_glBufferMock.Object);
        }

        private Factory<Vbo<FakeVertex>, VboArgs<FakeVertex>> _factory;
        private Mock<GlBuffer> _glBufferMock;
        private FakeVertex[] _vertices;

        [Test]
        public void BindsBuffer()
        {
            _factory.Create(new VboArgs<FakeVertex>(_vertices));
            _glBufferMock.Verify(m => m.WithBoundBuffer(BufferTarget.ArrayBuffer, 2, It.IsAny<Action>()));
        }

        [Test]
        public void BuffersData()
        {
            int size = Marshal.SizeOf<FakeVertex>();
            _glBufferMock.Setup(m => m.BufferData(BufferTarget.ArrayBuffer, size, _vertices, BufferUsageHint.StaticDraw))
                .Callback<BufferTarget, int, FakeVertex[], BufferUsageHint>((bufferTarget, bufferId, vertices, bufferUsageHint) =>
                {
                    _glBufferMock.Verify(m => m.WithBoundBuffer(BufferTarget.ArrayBuffer, 2, It.IsAny<Action>()));
                });

            _factory.Create(new VboArgs<FakeVertex>(_vertices));
            _glBufferMock.Verify(m => m.BufferData(BufferTarget.ArrayBuffer, size, _vertices, BufferUsageHint.StaticDraw));
        }

        [Test]
        public void DetectsOpenGlError()
        {
            _glBufferMock.Setup(m => m.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize))
                .Callback<BufferTarget, BufferParameterName>((bufferTarget, bufferParameterName) =>
                {
                    _glBufferMock.Verify(m => m.BufferData(BufferTarget.ArrayBuffer, It.IsAny<int>(), _vertices, It.IsAny<BufferUsageHint>()));
                })
                .Returns(0);

            Assert.Throws<Exception>(() => _factory.Create(new VboArgs<FakeVertex>(_vertices)));
        }

        [Test]
        public void GeneratesBuffer()
        {
            _factory.Create(new VboArgs<FakeVertex>(_vertices));
            _glBufferMock.Verify(m => m.GenerateBuffers(1));
        }

        [Test]
        public void ReturnsVbo()
        {
            Vbo<FakeVertex> vbo = _factory.Create(new VboArgs<FakeVertex>(_vertices));
            Assert.AreEqual(2, vbo.VboId);
        }
    }
}