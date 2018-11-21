namespace App
{
    public struct Quad<T>
    {
        public T FirstVertex { get; }
        public T SecondVertex { get; }
        public T ThirdVertex { get; }
        public T FourthVertex { get; }

        public Quad(T firstVertex, T secondVertex, T thirdVertex, T fourthVertex)
        {
            FirstVertex = firstVertex;
            SecondVertex = secondVertex;
            ThirdVertex = thirdVertex;
            FourthVertex = fourthVertex;
        }
    }
}