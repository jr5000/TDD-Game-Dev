namespace App
{
    public struct VaoArgs<T>
    {
        public VaoArgs(int vboId)
        {
            VboId = vboId;
        }

        public int VboId { get; }
    }
}