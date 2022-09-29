// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging.Wsq.IO;

namespace BiomSharp.Imaging.Wsq.Segment
{
    internal class Sob : DataSegment
    {
        // SOB: (16 bits) start of block marker; marks the beginning of the block header.
        public override Marker Marker => Marker.SOB;
        // Ls:  (16 bits) subband header length; specifies the length of the block header.
        // => @base.Size
        // Td:  (8 bits) Huffman coding table selector; selects one of eight possible 
        //      entropy coding tables needed for decoding the subbands within the segment.
        public byte Td { get; private set; }

        private Sob() { }

        public Sob(int td)
        {
            Td = (byte)td;
            // sizeof(byte)
            ContentSize = 1;
        }

        protected override void Read(EndianBinaryReader reader, Marker marker)
        {
            base.Read(reader, marker);
            Td = reader.ReadByte();
            Deserialized = true;
        }

        public override void Write(EndianBinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(Td);
        }
    }
}
