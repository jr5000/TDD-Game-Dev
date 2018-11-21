namespace App
{
    public class Vbo<T>
    {
        public Vbo(int vboId)
        {
            VboId = vboId;
        }

        public int VboId { get; }
    }
}