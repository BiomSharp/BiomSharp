// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging.Wsq.Huffman;
using BiomSharp.Imaging.Wsq.IO;

namespace BiomSharp.Imaging.Wsq.Segment
{
    internal class DhtTable
    {
        public const int MaxHuffBits = 16;
        public const int MaxHuffCodes = 256;
        public const int ZeroCodeSize = MaxHuffBits + 1;
        // Th:  (8 bits) Huffman table identifier; specifies one of eight possible destinations at the
        //      decoder into which the Huffman table shall be installed.
        public byte Th { get; private set; }
        // Li:  (8 bits) specifies the number of Huffman codes for each of the 16 possible lengths
        public byte[] L { get; private set; }
        // Vij: (8 bits) value associated with each Huffman code; specifies, for each i, the value
        //      associated with each Huffman code of length i.The meaning of each value is
        //      determined by the Huffman coding model.
        public byte[] V { get; private set; }

        public int CodeCount => L.Aggregate(0, (l1, l2) => l1 + l2);

        public int SerializeLength => ZeroCodeSize + CodeCount;

        public DhtTable()
        {
            L = new byte[MaxHuffBits];
            V = new byte[MaxHuffCodes + 1];
        }

        public DhtTable(int id)
            : this() => Th = (byte)id;

        public DhtTable(int id, int[] hBits, int[] hVals)
            : this()
        {
            Th = (byte)id;
            for (int i = 0; i < MaxHuffBits; i++)
            {
                L[i] = (byte)(hBits[i] & 0xFF);
            }
            if (hVals.Length > MaxHuffCodes + 1)
            {
                throw new WsqCodecException("Huffman code count exceeded 257");
            }
            for (int i = 0; i < hVals.Length; i++)
            {
                V[i] = (byte)(hVals[i] & 0xFF);
            }
        }

        public int[] GetCodeSizes()
        {
            int[] cs = new int[MaxHuffBits];
            for (int i = 0; i < L.Length; i++)
            {
                cs[i] = L[i];
            }
            return cs;
        }
        private void Read(EndianBinaryReader reader, Dht dht)
        {
            Th = reader.ReadByte();
            for (int i = 0; i < MaxHuffBits; i++)
            {
                L[i] = reader.ReadByte();
            }
            int sizeToRead = dht.SizeToRead() - ZeroCodeSize;
            if (CodeCount > sizeToRead)
            {
                throw new WsqCodecException(
                    "Huffman code count exceeds reported length in DHT header");
            }
            if (CodeCount > MaxHuffCodes + 1)
            {
                throw new WsqCodecException("Huffman code count > 257");
            }
            for (int j = 0; j < CodeCount; j++)
            {
                V[j] = reader.ReadByte();
            }
        }

        public static DhtTable CreateRead(EndianBinaryReader reader, Dht dht)
        {
            var table = new DhtTable();
            table.Read(reader, dht);
            return table;
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(Th);
            writer.Write(L);
            writer.Write(new Span<byte>(V, 0, CodeCount).ToArray());
        }
    }

    internal class Dht : DataSegment
    {
        public const int MaxHuffBits = 16;
        public const int MaxHuffTables = 8;
        // DHT: (16 bits) define Huffman table marker; marks the beginning of Huffman table
        //      definition parameters.
        public override Marker Marker => Marker.DHT;
        // Lh:  (16 bits) Huffman table definition length; specifies the length of all Huffman table
        //      parameters
        // => @base.Size
        private readonly List<DhtTable> tables = new();
        public IReadOnlyList<DhtTable> Tables => tables;

        private Dht() { }

        public Dht(int dhtId, int[] hBits, int[] hVals) : this() => AddTable(new DhtTable(dhtId, hBits, hVals));

        private int GetCodeCount() => tables.Sum(t => t.CodeCount);
        public void AddTable(DhtTable table)
        {
            if (tables.Any(t => t.Th == table.Th))
            {
                throw new WsqCodecException(string.Format(
                    "Dht table with Id= '{0}' already added", table.Th));
            }
            tables.Add(table);
            ContentSize = ((1 + MaxHuffBits) * tables.Count) + GetCodeCount();
        }

        public int SizeToRead() => ReadSize - GetCodeCount() - ((1 + MaxHuffBits) * tables.Count);

        public int ReadSize { get; private set; }
        private void Create(EndianBinaryReader reader)
        {
            _ = CodeEntries.Create(reader, this);
            while (SizeToRead() > 0)
            {
                _ = CodeEntries.Create(reader, this);
            }
        }

        protected override void Read(EndianBinaryReader reader, Marker marker)
        {
            base.Read(reader, marker);
            ReadSize = ContentSize;
            Create(reader);
            Deserialized = true;
        }

        public override void Write(EndianBinaryWriter writer)
        {
            base.Write(writer);
            tables.ForEach(t => t.Write(writer));
        }
    }
}
