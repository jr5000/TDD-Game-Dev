using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace App
{
    public interface GlClear
    {
        void WithClearColor(Color4 color, Action action);
        void WithCleared(ClearBufferMask clearBufferMask, Action action);
    }
}