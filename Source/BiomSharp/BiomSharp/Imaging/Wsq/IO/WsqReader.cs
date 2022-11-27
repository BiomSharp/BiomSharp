// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.Text;

namespace BiomSharp.Imaging.Wsq.IO
{
    internal class EndianBinaryReader : IDisposable
    {
        public Stream BaseStream { get; private set; }

        public EndianBinaryReader(Stream stream) => BaseStream = stream;

        // WSQ is big endian encoded.
        private byte[] ReadBigEndian(int len)
        {
            byte[] buffer = new byte[len];
            _ = BaseStream.Read(buffer, 0, len);
            int i = 0;
            int j = buffer.Length - 1;
            while (i < j)
            {
                (buffer[j], buffer[i]) = (buffer[i], buffer[j]);
                i++;
                j--;
            }
            return buffer;
        }

        public byte ReadByte()
        {
            byte[] b = new byte[1];
            _ = BaseStream.Read(b, 0, 1);
            return b[0];
        }

        public short ReadInt16() => BitConverter.ToInt16(ReadBigEndian(2), 0);

        public ushort ReadUInt16() => BitConverter.ToUInt16(ReadBigEndian(2), 0);

        public int ReadInt32() => BitConverter.ToInt32(ReadBigEndian(4), 0);

        public uint ReadUInt32() => BitConverter.ToUInt32(ReadBigEndian(4), 0);

        public string ReadString(int len)
        {
            byte[] buffer = new byte[len];
            _ = BaseStream.Read(buffer, 0, len);
            return Encoding.ASCII.GetString(buffer, 0, len);
        }

        #region IDisposable implementation

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    BaseStream.Dispose();
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
