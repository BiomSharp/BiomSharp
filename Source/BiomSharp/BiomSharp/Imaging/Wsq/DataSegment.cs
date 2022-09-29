// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging.Wsq.IO;

namespace BiomSharp.Imaging.Wsq
{
    internal abstract class DataSegment : BaseSegment
    {
        public int ContentSize { get; protected set; }

        protected DataSegment() { }

        protected override void Read(EndianBinaryReader reader, Marker marker)
        {
            base.Read(reader, marker);
            ContentSize = (ushort)(reader.ReadUInt16() - sizeof(ushort));
        }

        public override void Write(EndianBinaryWriter writer)
        {
            base.Write(writer);
            writer.Write((ushort)(ContentSize + sizeof(ushort)));
        }
    }
}
