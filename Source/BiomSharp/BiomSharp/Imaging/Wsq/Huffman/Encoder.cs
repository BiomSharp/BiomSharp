// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Imaging.Wsq.IO;

namespace BiomSharp.Imaging.Wsq.Huffman
{
    internal class Encoder
    {
        public const int MaxHuffBits = 16;
        public const int MaxHuffCounts = 256;
        public const int MaxHuffCoeff = 74;
        public const int CoeffCode = 0;
        public const int RunCode = 1;
        public const int MaxHuffZRun = 100;

        public struct BlockParms
        {
            public CodeEntries Codes { get; set; }
            public int[] Bits { get; set; }
            public int[] Values { get; set; }
        };

        private Encoder() { }

        public static Encoder Create() => new();

        private static int[] CountCodeFrequencies(WsqPixelBuffer<short> sImage, int offset, int length)
        {
            int rcnt = 0;
            int[] cnts = new int[MaxHuffCounts + 1];
            cnts[MaxHuffCounts] = 1;
            int LoMaxCoeff = 1 - MaxHuffCoeff;
            int state = CoeffCode;
            short[] pixels = sImage.Pixels;

            for (int cnt = offset; cnt < length + offset; cnt++)
            {
                short pix = pixels[cnt];
                switch (state)
                {
                    case CoeffCode:
                        if (pix == 0)
                        {
                            state = RunCode;
                            rcnt = 1;
                            break;
                        }
                        if (pix > MaxHuffCoeff)
                        {
                            if (pix > 255)
                            {
                                cnts[103]++;
                            }
                            else
                            {
                                cnts[101]++;
                            }
                        }
                        else if (pix < LoMaxCoeff)
                        {
                            if (pix < -255)
                            {
                                cnts[104]++;
                            }
                            else
                            {
                                cnts[102]++;
                            }
                        }
                        else
                        {
                            cnts[pix + 180]++;
                        }
                        break;

                    case RunCode:
                        if (pix == 0 && rcnt < 0xFFFF)
                        {
                            ++rcnt;
                            break;
                        }
                        if (rcnt <= MaxHuffZRun)
                        {
                            cnts[rcnt]++;
                        }
                        else if (rcnt <= 0xFF)
                        {
                            cnts[105]++;
                        }
                        else if (rcnt <= 0xFFFF)
                        {
                            cnts[106]++;
                        }
                        else
                        {
                            throw new WsqCodecException(
                                "Huffman encoder Z-Run too long in count block");
                        }
                        if (pix != 0)
                        {
                            if (pix > MaxHuffCoeff)
                            {
                                if (pix > 255)
                                {
                                    cnts[103]++;
                                }
                                else
                                {
                                    cnts[101]++;
                                }
                            }
                            else if (pix < LoMaxCoeff)
                            {
                                if (pix < -255)
                                {
                                    cnts[104]++;
                                }
                                else
                                {
                                    cnts[102]++;
                                }
                            }
                            else
                            {
                                cnts[pix + 180]++;
                            }
                            state = CoeffCode;
                        }
                        else
                        {
                            rcnt = 1;
                            state = RunCode;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (state == RunCode)
            {
                if (rcnt <= MaxHuffZRun)
                {
                    cnts[rcnt]++;
                }
                else if (rcnt <= 0xFF)
                {
                    cnts[105]++;
                }
                else if (rcnt <= 0xFFFF)
                {
                    cnts[106]++;
                }
                else
                {
                    throw new WsqCodecException(
                        "Huffman encoder Z-Run too long in count block");
                }
            }
            return cnts;
        }

        private static void GetLeastFrequency(out int first, out int next, int[] freq)
        {
            int code_temp;
            int value_temp;
            int code2 = int.MaxValue;
            int code1 = int.MaxValue;
            int set = 1;

            int value1_tmp = -1;
            int value2_tmp = -1;

            for (int i = 0; i <= MaxHuffCounts; i++)
            {
                if (freq[i] == 0)
                {
                    continue;
                }
                if (set == 1)
                {
                    code1 = freq[i];
                    value1_tmp = i;
                    set++;
                    continue;
                }
                if (set == 2)
                {
                    code2 = freq[i];
                    value2_tmp = i;
                    set++;
                }
                code_temp = freq[i];
                value_temp = i;
                if (code1 < code_temp && code2 < code_temp)
                {
                    continue;
                }
                if (code_temp < code1 || (code_temp == code1 && value_temp > value1_tmp))
                {
                    code2 = code1;
                    value2_tmp = value1_tmp;
                    code1 = code_temp;
                    value1_tmp = value_temp;
                    continue;
                }
                if (code_temp < code2 || (code_temp == code2 && value_temp > value1_tmp))
                {
                    code2 = code_temp;
                    value2_tmp = value_temp;
                }
            }
            first = value1_tmp;
            next = value2_tmp;
        }

        private static int[] GetCodeSizes(int[] codeFreqs)
        {
            int[] codeSizes = new int[MaxHuffCounts + 1];
            int[] others = new int[MaxHuffCounts + 1];
            for (int i = 0; i <= MaxHuffCounts; i++)
            {
                others[i] = -1;
            }
            while (true)
            {
                GetLeastFrequency(out int first, out int next, codeFreqs);
                if (next == -1)
                {
                    break;
                }
                codeFreqs[first] += codeFreqs[next];
                codeFreqs[next] = 0;
                codeSizes[first]++;
                while (others[first] != -1)
                {
                    first = others[first];
                    codeSizes[first]++;
                }
                others[first] = next;
                codeSizes[next]++;
                while (others[next] != -1)
                {
                    next = others[next];
                    codeSizes[next]++;
                }
            }
            return codeSizes;
        }

        private static int[] GetHuffBits(out bool adjust, int[] codeSizes)
        {
            adjust = false;
            int[] adSizes = new int[2 * MaxHuffBits];
            for (int i = 0; i < MaxHuffCounts; i++)
            {
                if (codeSizes[i] != 0)
                {
                    adSizes[codeSizes[i] - 1]++;
                }
                if (codeSizes[i] > MaxHuffBits)
                {
                    adjust = true;
                }
            }
            return adSizes;
        }

        private static void SortHuffBits(int[] bits)
        {
            int i, j;
            int l1, l2, l3;

            l3 = MaxHuffBits << 1;
            l1 = l3 - 1;
            l2 = MaxHuffBits - 1;

            int[] tbits = new int[l3];

            for (i = 0; i < MaxHuffBits << 1; i++)
            {
                tbits[i] = bits[i];
            }

            for (i = l1; i > l2; i--)
            {
                while (tbits[i] > 0)
                {
                    j = i - 2;
                    while (tbits[j] == 0)
                    {
                        j--;
                    }
                    tbits[i] -= 2;
                    tbits[i - 1] += 1;
                    tbits[j + 1] += 2;
                    tbits[j] -= 1;
                }
                tbits[i] = 0;
            }

            while (tbits[i] == 0)
            {
                i--;
            }

            tbits[i] -= 1;

            for (i = 0; i < MaxHuffBits << 1; i++)
            {
                bits[i] = tbits[i];
            }

            for (i = MaxHuffBits; i < l3; i++)
            {
                if (bits[i] > 0)
                {
                    throw new WsqCodecException(
                        "Huffman code length of is greater than 16.");
                }
            }
        }

        private static int[] SortCodeSizes(int[] codeSizes)
        {
            int[] sorted = new int[MaxHuffCounts + 1];
            int i2 = 0;
            for (int i = 1; i <= MaxHuffBits << 1; i++)
            {
                for (int i3 = 0; i3 < MaxHuffCounts; i3++)
                {
                    if (codeSizes[i3] == i)
                    {
                        sorted[i2] = i3;
                        i2++;
                    }
                }
            }
            return sorted;
        }

        private static CodeEntries CreateValueTable(CodeEntries bitEntries, int[] huffValues)
        {
            var valEntries = CodeEntries.Create(0);
            CodeEntry[] vcs = valEntries.Codes;
            CodeEntry[] bcs = bitEntries.Codes;
            for (int size = 0; size < bitEntries.Size; size++)
            {
                vcs[huffValues[size]].Code = bcs[size].Code;
                vcs[huffValues[size]].Size = bcs[size].Size;
            }
            return valEntries;
        }

        private static CodeEntries CreateCodes(
            out int[] huffBits,
            out int[] huffVals,
            WsqPixelBuffer<short> sImage, int offset,
            int[] blockSizes)
        {
            int[] codeFreqs = CountCodeFrequencies(sImage, offset, blockSizes[0]);
            for (int i = 1; i < blockSizes.Length; i++)
            {
                int[] codeFreqs2 = CountCodeFrequencies(sImage, offset + blockSizes[i - 1], blockSizes[i]);
                for (int j = 0; j < MaxHuffCounts; j++)
                {
                    codeFreqs[j] += codeFreqs2[j];
                }
            }
            int[] codeSizes = GetCodeSizes(codeFreqs);
            huffBits = GetHuffBits(out bool adjust, codeSizes);
            if (adjust)
            {
                SortHuffBits(huffBits);
            }
            huffVals = SortCodeSizes(codeSizes);
            var bitEntries = CodeEntries.Create(huffBits);
            CodeEntries valEntries = CreateValueTable(bitEntries, huffVals);
            return valEntries;
        }

        public static void CompressBlock(EndianBinaryWriter writer,
            WsqPixelBuffer<short> sImage,
            int offset, int length, CodeEntries codeEntries)
        {
            int loMaxCoeff;
            int pix;
            int rcnt = 0, state;
            int cnt;
            short[] sip = sImage.Pixels;
            CodeEntry[] codes = codeEntries.Codes;
            loMaxCoeff = 1 - MaxHuffCoeff;

            int outbit = 7;
            int bytes = 0;
            int bits = 0;

            state = CoeffCode;
            for (cnt = offset; cnt < length + offset; cnt++)
            {
                pix = sip[cnt];

                switch (state)
                {
                    case CoeffCode:
                        if (pix == 0)
                        {
                            state = RunCode;
                            rcnt = 1;
                            break;
                        }
                        if (pix > MaxHuffCoeff)
                        {
                            if (pix > 255)
                            {
                                /* 16bit pos esc */
                                WriteBits(writer, codes[103].Size, codes[103].Code,
                                    ref outbit, ref bits, ref bytes);
                                WriteBits(writer, 16, pix, ref outbit, ref bits, ref bytes);
                            }
                            else
                            {
                                /* 8bit pos esc */
                                WriteBits(writer, codes[101].Size, codes[101].Code,
                                    ref outbit, ref bits, ref bytes);
                                WriteBits(writer, 8, pix, ref outbit, ref bits, ref bytes);
                            }
                        }
                        else if (pix < loMaxCoeff)
                        {
                            if (pix < -255)
                            {
                                /* 16bit neg esc */
                                WriteBits(writer, codes[104].Size, codes[104].Code,
                                    ref outbit, ref bits, ref bytes);
                                WriteBits(writer, 16, -pix, ref outbit, ref bits, ref bytes);
                            }
                            else
                            {
                                /* 8bit neg esc */
                                WriteBits(writer, codes[102].Size, codes[102].Code,
                                    ref outbit, ref bits, ref bytes);
                                WriteBits(writer, 8, -pix, ref outbit, ref bits, ref bytes);
                            }
                        }
                        else
                        {
                            /* within table */
                            WriteBits(writer, codes[pix + 180].Size, codes[pix + 180].Code,
                                ref outbit, ref bits, ref bytes);
                        }
                        break;

                    case RunCode:
                        if (pix == 0 && rcnt < 0xFFFF)
                        {
                            ++rcnt;
                            break;
                        }
                        if (rcnt <= MaxHuffZRun)
                        {
                            /* log zero run length */
                            WriteBits(writer, codes[rcnt].Size, codes[rcnt].Code,
                                ref outbit, ref bits, ref bytes);
                        }
                        else if (rcnt <= 0xFF)
                        {
                            /* 8bit zrun esc */
                            WriteBits(writer, codes[105].Size, codes[105].Code,
                                ref outbit, ref bits, ref bytes);
                            WriteBits(writer, 8, rcnt, ref outbit, ref bits, ref bytes);
                        }
                        else if (rcnt <= 0xFFFF)
                        {
                            /* 16bit zrun esc */
                            WriteBits(writer, codes[106].Size, codes[106].Code,
                                ref outbit, ref bits, ref bytes);
                            WriteBits(writer, 16, rcnt, ref outbit, ref bits, ref bytes);
                        }
                        else
                        {
                            throw new WsqCodecException(
                                "ERROR : CompressBlock : zrun too large.");
                        }

                        if (pix != 0)
                        {
                            if (pix > MaxHuffCoeff)
                            {
                                /* log current pix */
                                if (pix > 255)
                                {
                                    /* 16bit pos esc */
                                    WriteBits(writer, codes[103].Size, codes[103].Code,
                                        ref outbit, ref bits, ref bytes);
                                    WriteBits(writer, 16, pix, ref outbit, ref bits, ref bytes);
                                }
                                else
                                {
                                    /* 8bit pos esc */
                                    WriteBits(writer, codes[101].Size, codes[101].Code,
                                        ref outbit, ref bits, ref bytes);
                                    WriteBits(writer, 8, pix, ref outbit, ref bits, ref bytes);
                                }
                            }
                            else if (pix < loMaxCoeff)
                            {
                                if (pix < -255)
                                {
                                    /* 16bit neg esc */
                                    WriteBits(writer, codes[104].Size, codes[104].Code,
                                        ref outbit, ref bits, ref bytes);
                                    WriteBits(writer, 16, -pix, ref outbit, ref bits, ref bytes);
                                }
                                else
                                {
                                    /* 8bit neg esc */
                                    WriteBits(writer, codes[102].Size, codes[102].Code,
                                        ref outbit, ref bits, ref bytes);
                                    WriteBits(writer, 8, -pix, ref outbit, ref bits, ref bytes);
                                }
                            }
                            else
                            {
                                /* within table */
                                WriteBits(writer, codes[pix + 180].Size, codes[pix + 180].Code,
                                    ref outbit, ref bits, ref bytes);
                            }
                            state = CoeffCode;
                        }
                        else
                        {
                            rcnt = 1;
                            state = RunCode;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (state == RunCode)
            {
                if (rcnt <= MaxHuffZRun)
                {
                    WriteBits(writer, codes[rcnt].Size, codes[rcnt].Code, ref outbit, ref bits, ref bytes);
                }
                else if (rcnt <= 0xFF)
                {
                    WriteBits(writer, codes[105].Size, codes[105].Code, ref outbit, ref bits, ref bytes);
                    WriteBits(writer, 8, rcnt, ref outbit, ref bits, ref bytes);
                }
                else if (rcnt <= 0xFFFF)
                {
                    WriteBits(writer, codes[106].Size, codes[106].Code, ref outbit, ref bits, ref bytes);
                    WriteBits(writer, 16, rcnt, ref outbit, ref bits, ref bytes);
                }
                else
                {
                    throw new WsqCodecException("CompressBlock : zrun2 too large.");
                }
            }
            FlushBits(writer, ref outbit, ref bits, ref bytes);
        }

        private static void WriteBits(EndianBinaryWriter writer, int size, int code,
            ref int outbit, ref int bits, ref int bytes)
        {
            int num = size;
            for (--num; num >= 0; num--)
            {
                int tempValue = bits;
                tempValue <<= 1;
                tempValue |= (code >> num) & 0x0001 & 0xFF;
                bits = tempValue;
                outbit--;
                if (outbit < 0)
                {
                    writer.Write((byte)bits);
                    if ((bits & 0xFF) == 0xFF)
                    {
                        writer.Write((byte)0);
                        bytes++;
                    }
                    bytes++;
                    outbit = 7;
                    bits = 0;
                }
            }
        }

        private static void FlushBits(EndianBinaryWriter writer, ref int outbit, ref int bits, ref int bytes)
        {
            if (outbit != 7)
            {
                for (int cnt = outbit; cnt >= 0; cnt--)
                {
                    int tempValue = bits;
                    tempValue <<= 1;
                    tempValue |= 0x01;
                    bits = tempValue;
                }
                writer.Write((byte)bits);
                if (bits == 0xFF)
                {
                    writer.Write((byte)0);
                    bytes++;
                }
                bytes++;
                outbit = 7;
                bits = 0;
            }
        }

        public static IList<BlockParms> Encode(WsqPixelBuffer<short> sImage, int[] quantSizes)
        {
            var blockParms = new List<BlockParms>();
            CodeEntries codes = CreateCodes(out int[] huffBits, out int[] huffVals, sImage, 0,
                new int[] { quantSizes[0] });
            blockParms.Add(new BlockParms
            {
                Bits = huffBits,
                Codes = codes,
                Values = huffVals,
            });
            codes = CreateCodes(out huffBits, out huffVals, sImage, quantSizes[0],
                new int[] { quantSizes[1], quantSizes[2] });
            blockParms.Add(new BlockParms
            {
                Bits = huffBits,
                Codes = codes,
                Values = huffVals,
            });
            return blockParms;
        }
    }
}
