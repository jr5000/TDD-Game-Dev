using OpenTK;

namespace App
{
    public class QuadRenderer<T> : Renderer
    {
        private readonly Ebo _ebo;
        private readonly Vao<T> _vao;
        private readonly Vbo<T> _vbo;
        private readonly VboRenderer<T> _vboRenderer;

        public QuadRenderer(Quad<T>[] quads, Factory<Vao<T>, VaoArgs<T>> vaoFactory, Factory<Vbo<T>, VboArgs<T>> vboFactory,
            Factory<Ebo, EboArgs> eboFactory, VboRenderer<T> vboRenderer)
        {
            _vboRenderer = vboRenderer;
            var vertices = new T[quads.Length * 4];
            var indices = new uint[quads.Length * 6];
            for (uint i = 0; i < quads.Length; i++)
            {
                vertices[i * 4] = quads[i].FirstVertex;
                vertices[i * 4 + 1] = quads[i].SecondVertex;
                vertices[i * 4 + 2] = quads[i].ThirdVertex;
                vertices[i * 4 + 3] = quads[i].FourthVertex;

                indices[i * 6] = i * 4;
                indices[i * 6 + 1] = i * 4 + 2;
                indices[i * 6 + 2] = i * 4 + 3;
                indices[i * 6 + 3] = i * 4;
                indices[i * 6 + 4] = i * 4 + 1;
                indices[i * 6 + 5] = i * 4 + 2;
            }

            _vbo = vboFactory.Create(new VboArgs<T>(vertices));

            _vao = vaoFactory.Create(new VaoArgs<T>(_vbo.VboId));

            _ebo = eboFactory.Create(new EboArgs(indices));
        }

        public void Render(FrameEventArgs e)
        {
            _vboRenderer.Render(_vao, _vbo, _ebo);
        }
    }
}