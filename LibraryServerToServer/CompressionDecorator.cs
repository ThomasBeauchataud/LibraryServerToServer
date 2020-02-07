using System.IO;
using System.IO.Compression;

namespace LibraryServerToServer
{
    public class CompressionDecorator : DataSourceDecorator
    {
        public CompressionDecorator(IDataSource dataSource) : base(dataSource)
        {
        }

        public new void WriteData(byte[] data)
        {
            base.WriteData(Zip(data));
        }

        public new byte[] ReadData()
        {
            return Unzip(base.ReadData());
        }

        public byte[] Zip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    CopyTo(msi, gs);
                }
                return mso.ToArray();
            }
        }

        private byte[] Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }
                return mso.ToArray();
            }
        }

        private void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[10000];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
    }
}
