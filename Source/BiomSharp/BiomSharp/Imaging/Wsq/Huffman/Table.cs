// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Imaging.Wsq.Huffman
{
    public struct TableEntry
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public int ValIndex { get; set; }
    }

    internal class Table
    {
        public TableEntry[]? Entries { get; private set; }
        public int Count { get; private set; }

        public Table() { }

        private void Build(Segment.DhtTable dhtTable, CodeEntries hcs)
        {
            Entries = Enumerable
                .Range(0, Segment.DhtTable.MaxHuffBits + 1)
                .Select(dt => new TableEntry())
                .ToArray();
            int i2 = 0;
            for (int i = 1; i <= Segment.DhtTable.MaxHuffBits; i++)
            {
                if (dhtTable.L[i - 1] == 0)
                {
                    Entries[i].Max = -1;
                    continue;
                }
                Entries[i].ValIndex = i2;
                Entries[i].Min = hcs.Codes[i2].Code;
                i2 = i2 + dhtTable.L[i - 1] - 1;
                Entries[i].Max = hcs.Codes[i2].Code;
                ++i2;
            }
        }

        public static Table Create(Segment.DhtTable dhtTable, CodeEntries hcs)
        {
            var dt = new Table();
            dt.Build(dhtTable, hcs);
            return dt;
        }
    }
}
