using System;
using OpenTK.Graphics.OpenGL;

namespace App
{
    public interface GlBuffer
    {
        void BufferData<T>(BufferTarget bufferTarget, int size, T[] data, BufferUsageHint bufferUsageHint) where T : struct;
        int[] GenerateBuffers(int numBuffers);
        int GetBufferParameter(BufferTarget bufferTarget, BufferParameterName bufferParameterName);
        void WithBoundBuffer(BufferTarget bufferTarget, int bufferId, Action action);
    }
}