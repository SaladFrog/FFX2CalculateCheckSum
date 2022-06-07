namespace CalculateCheckSum
{
    public static class Extensions
    {
        public static void Write(this BinaryWriter bw, int offset, short value)
        {
            bw.BaseStream.Seek(offset, SeekOrigin.Begin);
            bw.Write(value);
        }
    }
}
