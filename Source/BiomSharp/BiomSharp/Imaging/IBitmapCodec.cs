// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

namespace BiomSharp.Imaging
{
    public interface IBitmapCodec
    {
        string[]? FileExtensions { get; }

        void FromRaw(SimpleBitmap? raw);

        SimpleBitmap? ToRaw();

        byte[]? Encode();

        byte[]? Encode<TParms>(TParms? writeParms) where TParms : class, new();

        public void Decode(byte[] encoded);

        public void Decode<TParms>(byte[] encoded, out TParms? readParms) where TParms : class, new();

        public void Read(Stream stream);

        public void Read<TParms>(Stream stream, out TParms? readParms) where TParms : class, new();

        public void Write(Stream stream);

        public void Write<TParms>(Stream stream, TParms? writeParms) where TParms : class, new();
    }

    public interface IBitmapCodec<TParms> where TParms : class, new()
    {
        byte[]? Encode(TParms? writeParms);

        public void Decode(byte[] encoded, out TParms? readParms);

        public void Read(Stream stream, out TParms? readParms);

        public void Write(Stream stream, TParms? writeParms);
    }
}
