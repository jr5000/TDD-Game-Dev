namespace App
{
    public interface VboRenderer<T>
    {
        void Render(Vao<T> vao, Vbo<T> vbo, Ebo ebo);
    }
}