namespace CalculateCheckSum
{
    public class Program
    {
        private static MemoryStream? ms;
        private static BinaryReader? br;
        private static BinaryWriter? bw;
        private static readonly int crcOffset = 0xd424;

        public static void Main(string[] args)
        {
            Console.WriteLine(args.Length);
            if(args.Length == 0 || args.Length > 1)
            {
                Console.WriteLine($"No files found");
                return;
            }
            string saveFile = Path.GetFullPath(args[0]);
            MemoryStream saveStream = new MemoryStream(File.ReadAllBytes(saveFile));
            ms = saveStream;
            br = new BinaryReader(ms);
            bw = new BinaryWriter(ms);

            bw.Write(crcOffset, 0);
            Crc16_CCITT hasher = new Crc16_CCITT();
            {
                br.BaseStream.Seek(0x40, SeekOrigin.Begin);
                hasher.ComputeHash(br.ReadBytes(crcOffset + 4 - 0x40));
                bw.Write(0x1a, (short)hasher.Value);
                bw.Write(crcOffset, (short)hasher.Value);
            }

            using (BinaryWriter fw = new BinaryWriter(File.Open(saveFile, FileMode.Create)))
            {
                fw.Write(ms.ToArray());
            }
        }
    }
}

