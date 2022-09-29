// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Text;

namespace BiomSharp.Imaging.Wsq.IO
{
    internal class EndianBinaryWriter : IDisposable
    {
        public Stream BaseStream { get; private set; }

        public EndianBinaryWriter(Stream stream) => BaseStream = stream;

        // WSQ is big endian encoded.
        private void WriteBigEndian(byte[] buffer)
        {
            int i = 0;
            int j = buffer.Length - 1;
            while (i < j)
            {
                (buffer[j], buffer[i]) = (buffer[i], buffer[j]);
                i++;
                j--;
            }
            BaseStream.Write(buffer, 0, buffer.Length);
        }

        public void Write(byte[] bytes) => BaseStream.Write(bytes, 0, bytes.Length);

        public void Write(byte b) => BaseStream.Write(new byte[] { b }, 0, 1);

        public void Write(short s) => WriteBigEndian(BitConverter.GetBytes(s));

        public void Write(ushort u) => WriteBigEndian(BitConverter.GetBytes(u));

        public void Write(int i) => WriteBigEndian(BitConverter.GetBytes(i));

        public void Write(uint u) => WriteBigEndian(BitConverter.GetBytes(u));

        public void Write(string s)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(s);
            BaseStream.Write(buffer, 0, buffer.Length);
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
