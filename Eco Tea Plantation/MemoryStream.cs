namespace Eco_Tea_Plantation
{
    internal class MemoryStream : System.IO.MemoryStream
    {
        public MemoryStream()
        {
        }

        public MemoryStream(byte[] buffer) : base(buffer)
        {
        }
    }
}