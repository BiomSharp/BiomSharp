// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Imaging.Wsq.IO;
using BiomSharp.Imaging.Wsq.Segment;

namespace BiomSharp.Imaging.Wsq.Huffman
{
    internal class Decoder
    {
        private static readonly byte[] bitmask = { 0x00, 0x01, 0x03, 0x07, 0x0F, 0x1F, 0x3F, 0x7F, 0xFF };
        private int decIdx;

        private Decoder() { }

        public static Decoder Create() => new();

        public void Decode(
            EndianBinaryReader reader,
            WsqPixelBuffer<int> hdc,
            DhtTable dhtTable, Table? hdt)
        {
            if (hdt != null && hdt.Entries != null)
            {
                Marker marker = Marker.None;
                int bitCount = 0;
                int nextByte = 0;
                long eos = reader.BaseStream.Length;
                int[] pix = hdc.Pixels;

                while (reader.BaseStream.Position < eos)
                {
                    short code = (short)ReadBits(reader, ref marker, ref bitCount, 1, ref nextByte);
                    if (marker != Marker.None)
                    {
                        _ = reader.BaseStream.Seek(-sizeof(Marker), SeekOrigin.Current);
                        return;
                    }
                    int inx;
                    for (inx = 1; code > hdt.Entries[inx].Max; inx++)
                    {
                        int tbits = ReadBits(reader, ref marker, ref bitCount, 1, ref nextByte);
                        code = (short)((code << 1) + tbits);
                        if (marker != Marker.None)
                        {
                            _ = reader.BaseStream.Seek(-sizeof(Marker), SeekOrigin.Current);
                            return;
                        }
                    }
                    int inx2 = hdt.Entries[inx].ValIndex + code - hdt.Entries[inx].Min;
                    int nodeIndex = dhtTable.V[inx2];

                    if (nodeIndex is > 0 and <= 100)
                    {
                        for (int n = 0; n < nodeIndex; n++)
                        {
                            pix[decIdx++] = 0;
                        }
                    }
                    else if (nodeIndex is > 106 and < 0xFF)
                    {
                        pix[decIdx++] = nodeIndex - 180;
                    }
                    else if (nodeIndex == 101)
                    {
                        pix[decIdx++] = ReadBits(reader, ref marker, ref bitCount, 8, ref nextByte);
                    }
                    else if (nodeIndex == 102)
                    {
                        pix[decIdx++] = -ReadBits(reader, ref marker, ref bitCount, 8, ref nextByte);
                    }
                    else if (nodeIndex == 103)
                    {
                        pix[decIdx++] = ReadBits(reader, ref marker, ref bitCount, 16, ref nextByte);
                    }
                    else if (nodeIndex == 104)
                    {
                        pix[decIdx++] = -ReadBits(reader, ref marker, ref bitCount, 16, ref nextByte);
                    }
                    else if (nodeIndex == 105)
                    {
                        int n = ReadBits(reader, ref marker, ref bitCount, 8, ref nextByte);
                        while (n-- > 0)
                        {
                            pix[decIdx++] = 0;
                        }
                    }
                    else if (nodeIndex == 106)
                    {
                        int n = ReadBits(reader, ref marker, ref bitCount, 16, ref nextByte);
                        while (n-- > 0)
                        {
                            pix[decIdx++] = 0;
                        }
                    }
                    else
                    {
                        throw new WsqCodecException("Huffman blocks invalid code");
                    }
                }
            }
            else
            {
                throw new WsqCodecException("Huffman table is null or invalid");
            }
        }

        private static int ReadBits(EndianBinaryReader reader,
            ref Marker marker, ref int bitCount, int bitsRequired, ref int nextByte)
        {
            if (bitCount == 0)
            {
                nextByte = reader.ReadByte();
                bitCount = 8;
                if (nextByte == 0xFF)
                {
                    int code2 = reader.ReadByte();
                    if (code2 != 0 && bitsRequired == 1)
                    {
                        marker = (Marker)((nextByte << 8) | code2);
                        return 1;
                    }
                    if (code2 != 0)
                    {
                        throw new WsqCodecException(
                            "Huffman decoding expected stuffed zeros");
                    }
                }
            }
            int bits;
            if (bitsRequired <= bitCount)
            {
                bits = (nextByte >> (bitCount - bitsRequired)) & bitmask[bitsRequired];
                bitCount -= bitsRequired;
                nextByte &= bitmask[bitCount];
            }
            else
            {
                int bitsNeeded = bitsRequired - bitCount;
                bits = nextByte << bitsNeeded;
                bitCount = 0;
                bits |= ReadBits(reader, ref marker, ref bitCount, bitsNeeded, ref nextByte);
            }
            return bits;
        }
    }
}
