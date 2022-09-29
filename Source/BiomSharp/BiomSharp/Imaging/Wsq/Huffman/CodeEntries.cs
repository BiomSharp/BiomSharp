// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging.Wsq.IO;
using BiomSharp.Imaging.Wsq.Segment;

namespace BiomSharp.Imaging.Wsq.Huffman
{
    internal struct CodeEntry
    {
        public int Size { get; set; }
        public int Code { get; set; }
    }

    internal class CodeEntries
    {
        public CodeEntry[] Codes { get; private set; }
        public int Size { get; private set; }

        private CodeEntries(int length) => Codes = Enumerable
                .Range(0, length)
                .Select(hc => new CodeEntry())
                .ToArray();

        private void CreateCodeSizes(DhtTable table)
        {
            int codes = 1;
            Size = 0;
            for (int i = 1; i <= DhtTable.MaxHuffBits; i++)
            {
                while (codes <= table.L[i - 1])
                {
                    Codes[Size].Size = (short)i;
                    ++Size;
                    ++codes;
                }
                codes = 1;
            }
            Codes[Size].Size = 0;
        }
        private void CreateCodes()
        {
            int ci = 0;
            short code = 0;
            int size = Codes[0].Size;
            if (Codes[ci].Size != 0)
            {
                do
                {
                    do
                    {
                        Codes[ci].Code = code;
                        ++code;
                        ++ci;
                    } while (Codes[ci].Size == size);
                    if (Codes[ci].Size != 0)
                    {
                        do
                        {
                            code <<= 1;
                            ++size;
                        } while (Codes[ci].Size != size);
                    }
                } while (Codes[ci].Size == size);
            }
        }
        public bool CheckAllOnes()
        {
            bool allOnes = false;
            for (int i = 0; i < Size; i++)
            {
                bool all1 = true;
                for (int k = 0; k < Codes[i].Size && all1; k++)
                {
                    all1 = all1 && ((Codes[i].Code >> k) & 0x0001) != 0;
                }
                if (all1)
                {
                    allOnes = all1;
                }
            }
            return allOnes;
        }

        private void CreateCodeSizes(int[] huffBits)
        {
            int codes = 1;
            Size = 0;
            for (int i = 1; i <= DhtTable.MaxHuffBits; i++)
            {
                while (codes <= huffBits[i - 1])
                {
                    Codes[Size].Size = (short)i;
                    ++Size;
                    ++codes;
                }
                codes = 1;
            }
            Codes[Size].Size = 0;
        }
        private void CheckTable()
        {
            if (CheckAllOnes())
            {
                System.Diagnostics.Debug.WriteLine(
                    "WSQ Warning: A code in the huffman table contains an all 1's code. " +
                    "This image may still be decodable. It is not compliant with " +
                    "the WSQ specification.");
            }
        }

        public static CodeEntries Create(int[] codeSizes)
        {
            var hcs = new CodeEntries(DhtTable.MaxHuffCodes + 1);
            hcs.CreateCodeSizes(codeSizes);
            hcs.CreateCodes();
            hcs.CheckTable();
            return hcs;
        }

        public static CodeEntries Create(EndianBinaryReader reader, Dht dht)
        {
            var hcs = new CodeEntries(DhtTable.MaxHuffCodes + 1);
            var dhtTable = DhtTable.CreateRead(reader, dht);
            dht.AddTable(dhtTable);
            hcs.CreateCodeSizes(dhtTable);
            hcs.CreateCodes();
            hcs.CheckTable();
            return hcs;
        }

        public static CodeEntries Create(int size) => new(DhtTable.MaxHuffCodes + 1)
        {
            Size = size,
        };
    }
}
