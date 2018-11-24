using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace App
{
    public class VboFactory<T> : Factory<Vbo<T>, VboArgs<T>> where T : struct
    {
        private readonly GlBuffer _glBuffer;

        public VboFactory(GlBuffer glBuffer)
        {
            _glBuffer = glBuffer;
        }

        public Vbo<T> Create(VboArgs<T> args)
        {
            int[] bufferIds = _glBuffer.GenerateBuffers(1);
            _glBuffer.WithBoundBuffer(BufferTarget.ArrayBuffer, bufferIds[0], () =>
            {
                int size = Marshal.SizeOf<T>();
                _glBuffer.BufferData(BufferTarget.ArrayBuffer, size, args.Vertices, BufferUsageHint.StaticDraw);

                int bufferSize = _glBuffer.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize);
                if (bufferSize != size * args.Vertices.Length)
                    throw new Exception("Vertex data not uploaded correctly");
            });
            return new Vbo<T>(bufferIds[0]);
        }
    }
}