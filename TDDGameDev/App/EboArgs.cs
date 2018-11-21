namespace App
{
    public struct EboArgs
    {
        public EboArgs(uint[] indices)
        {
            Indices = indices;
        }

        public uint[] Indices { get; }
    }
}