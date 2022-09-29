// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Plugins;

namespace BiomSharp.Imaging
{
    public abstract class BitmapCodec<TFormat>
        :
        IPlugin<TFormat>, IBitmapCodec
        where TFormat : Enum
    {
        public abstract TFormat? Id { get; }
        public abstract string? Description { get; }
        public abstract string[]? FileExtensions { get; }

        public abstract void Decode(byte[] encoded);
        public abstract void Decode<TParms>(byte[] encoded, out TParms? readParms) where TParms : class, new();
        public abstract byte[]? Encode();
        public abstract byte[]? Encode<TParms>(TParms? writeParms) where TParms : class, new();
        public abstract void FromRaw(SimpleBitmap? raw);
        public abstract void Read(Stream stream);
        public abstract void Read<TParms>(Stream stream, out TParms? readParms) where TParms : class, new();
        public abstract SimpleBitmap? ToRaw();
        public abstract void Write(Stream stream);
        public abstract void Write<TParms>(Stream stream, TParms? writeParms) where TParms : class, new();
    }
}
