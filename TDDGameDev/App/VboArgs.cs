namespace App
{
    public struct VboArgs<T>
    {
        public VboArgs(T[] vertices)
        {
            Vertices = vertices;
        }

        public T[] Vertices { get; }
    }
}