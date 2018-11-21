using System.Linq;
using App;
using Moq;
using NUnit.Framework;
using OpenTK;

namespace Tests
{
    [TestFixture]
    internal class QuadRendererTest
    {
        [SetUp]
        public void SetUp()
        {
            var fakeVertex = new FakeVertex();
            _quad = new Quad<FakeVertex>(fakeVertex, fakeVertex, fakeVertex, fakeVertex);
            _expectedVertices = new[]
            {
                _quad.FirstVertex, _quad.SecondVertex, _quad.ThirdVertex, _quad.FourthVertex,
                _quad.FirstVertex, _quad.SecondVertex, _quad.ThirdVertex, _quad.FourthVertex
            };
            _expectedIndices = new uint[]
            {
                0, 2, 3, 0, 1, 2,
                4, 6, 7, 4, 5, 6
            };
            _fakeVao = new Vao<FakeVertex>();
            _fakeVbo = new Vbo<FakeVertex>(3);
            _fakeEbo = new Ebo();
            _vaoFactoryMock = new Mock<Factory<Vao<FakeVertex>, VaoArgs<FakeVertex>>>();
            _vaoFactoryMock.Setup(m => m.Create(It.IsAny<VaoArgs<FakeVertex>>())).Returns(_fakeVao);
            _vboFactoryMock = new Mock<Factory<Vbo<FakeVertex>, VboArgs<FakeVertex>>>();
            _vboFactoryMock.Setup(m => m.Create(It.IsAny<VboArgs<FakeVertex>>())).Returns(_fakeVbo);
            _eboFactoryMock = new Mock<Factory<Ebo, EboArgs>>();
            _eboFactoryMock.Setup(m => m.Create(It.IsAny<EboArgs>())).Returns(_fakeEbo);
            _vboRendererMock = new Mock<VboRenderer<FakeVertex>>();
            _quadRenderer = new QuadRenderer<FakeVertex>(new[] {_quad, _quad}, _vaoFactoryMock.Object, _vboFactoryMock.Object, _eboFactoryMock.Object, _vboRendererMock.Object);
        }

        private Quad<FakeVertex> _quad;
        private Renderer _quadRenderer;
        private Mock<Factory<Vao<FakeVertex>, VaoArgs<FakeVertex>>> _vaoFactoryMock;
        private Mock<Factory<Vbo<FakeVertex>, VboArgs<FakeVertex>>> _vboFactoryMock;
        private Mock<Factory<Ebo, EboArgs>> _eboFactoryMock;
        private Mock<VboRenderer<FakeVertex>> _vboRendererMock;
        private Vbo<FakeVertex> _fakeVbo;
        private Ebo _fakeEbo;
        private Vao<FakeVertex> _fakeVao;
        private FakeVertex[] _expectedVertices;
        private uint[] _expectedIndices;

        [Test]
        public void CallsVboRendererOnRender()
        {
            _quadRenderer.Render(new FrameEventArgs());
            _vboRendererMock.Verify(m => m.Render(_fakeVao, _fakeVbo, _fakeEbo));
        }

        [Test]
        public void CreatesEbo()
        {
            _eboFactoryMock.Verify(m => m.Create(It.Is<EboArgs>(eboArgs =>
                eboArgs.Indices.SequenceEqual(_expectedIndices)
            )));
        }

        [Test]
        public void CreatesVao()
        {
            _vaoFactoryMock.Verify(m => m.Create(It.Is<VaoArgs<FakeVertex>>(vaoArgs =>
                vaoArgs.VboId == 3
            )));
        }

        [Test]
        public void CreatesVbo()
        {
            _vboFactoryMock.Verify(m => m.Create(It.Is<VboArgs<FakeVertex>>(vboArgs =>
                vboArgs.Vertices.SequenceEqual(_expectedVertices)
            )));
        }
    }
}