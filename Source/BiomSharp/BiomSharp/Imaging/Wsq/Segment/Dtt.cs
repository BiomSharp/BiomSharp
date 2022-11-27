// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Imaging.Wsq.IO;
using BiomSharp.Imaging.Wsq.Tree;

namespace BiomSharp.Imaging.Wsq.Segment
{
    // Serialized sizes shown in ()
    internal class DttCoefficient
    {
        // Sn: (8 bits) sign of the filter coefficient; zero is positive, nonzero is negative.
        public byte Sn { get; private set; }
        // Ex: (8 bits) scale exponent; the decimal point in the filter coefficient is 
        //      moved left Ex places.
        public byte Ex { get; private set; }
        // H:  (32 bits) filter element; specifies the filter 
        //      coefficients for the DWT, starting at the center of the filter (H01).
        public uint H { get; private set; }
        // L: Normalized coefficient.
        public float L { get; private set; }
        public DttCoefficient() { }
        // sizeof(uint) + 2 * sizeof(byte)
        public const int SerializeLength = 6;

        public void Read(EndianBinaryReader reader)
        {
            Sn = reader.ReadByte();
            Ex = reader.ReadByte();
            H = reader.ReadUInt32();
            float l = Math.FromBasePlusShift(H, Ex);
            L = Sn != 0 ? -l : l;
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(Sn);
            writer.Write(Ex);
            writer.Write(H);
        }

        public static DttCoefficient CreateRead(EndianBinaryReader reader)
        {
            var filt = new DttCoefficient();
            filt.Read(reader);
            return filt;
        }

        public DttCoefficient(float coeff)
        {
            L = coeff;
            long h0 = Math.ToBasePlusShiftLong(L, out int scale, out byte sign);
            if (h0 > uint.MaxValue)
            {
                throw new WsqCodecException("Dtt coefficient overflow (> 4294967295)");
            }
            Ex = (byte)(scale & 0xFF);
            Sn = sign;
            H = (uint)(h0 & 0xFFFFFFFFL);
        }
    }
    internal class Dtt : DataSegment
    {
        // DTT: (16 bits) define transform table marker; marks the beginning of the 
        //      transform table specification parameters.
        public override Marker Marker => Marker.DTT;
        // Lt:  (16 bits) transform table definition length; specifies the length of all 
        //      transform table parameters.
        // => @base.Size
        //public ushort Lt { get; private set; }
        // L0:  (8 bits) number of analysis lowpass filter coefficients(length of ho).
        public byte L0 { get; private set; }
        // L1:  (8 bits) number of analysis high pass filter coefficients(length of h1).
        public byte L1 { get; private set; }
        // L0k: 1..k normalized coefficients L0..Lk (= @DttCoefficient.L)
        public float[] DttL0 { get; private set; }
        // L1k: 1..k normalized coefficients L0..Lk (= @DttCoefficient.L)
        public float[] DttL1 { get; private set; }

        private Dtt() : this(Filter.Odd7x9) { }

        public Dtt(Filter filter)
        {
            DttL0 = filter.Lo;
            L0 = (byte)DttL0.Length;
            DttL1 = filter.Hi;
            L1 = (byte)DttL1.Length;
            // 2 * sizeof(byte) + DttCoefficient.SerializeLength * ((L0 + L1) / 2 + 1)
            ContentSize = 56;
        }

        private static float[] ReadCoefficients(EndianBinaryReader reader, int lSize, bool l1)
        {
            float[] ldtt = new float[lSize];
            int aSize = lSize % 2 != 0 ? (lSize + 1) / 2 : lSize / 2;
            var adtt = new DttCoefficient[aSize];
            --aSize;

            for (int i = 0; i <= aSize; i++)
            {
                adtt[i] = DttCoefficient.CreateRead(reader);
                if (lSize % 2 != 0)
                {
                    ldtt[i + aSize] = Math.SignOfPower(i) * adtt[i].L;
                    if (i > 0)
                    {
                        ldtt[aSize - i] = ldtt[i + aSize];
                    }
                }
                else
                {
                    if (l1)
                    {
                        //dtt_table.hifilt[cnt + a_size + 1] = int_sign(cnt) * a_lofilt[cnt];
                        //dtt_table.hifilt[a_size - cnt] = -1 * dtt_table.hifilt[cnt + a_size + 1];
                        ldtt[i + aSize + 1] = Math.SignOfPower(i) * adtt[i].L;
                        ldtt[aSize - i] = -ldtt[i + aSize + 1];
                    }
                    else
                    {
                        //dtt_table.lofilt[cnt + a_size + 1] = int_sign(cnt + 1) * a_hifilt[cnt];
                        //dtt_table.lofilt[a_size - cnt] = dtt_table.lofilt[cnt + a_size + 1];
                        ldtt[i + aSize + 1] = Math.SignOfPower(i + 1) * adtt[i].L;
                        ldtt[aSize - i] = ldtt[i + aSize + 1];
                    }
                }
            }
            return ldtt;
        }

        private static void WriteCoefficients(EndianBinaryWriter writer, float[] coeffs)
        {
            for (int coef = coeffs.Length >> 1; coef < coeffs.Length; coef++)
            {
                new DttCoefficient(coeffs[coef]).Write(writer);
            }
        }

        protected override void Read(EndianBinaryReader reader, Marker marker)
        {
            base.Read(reader, marker);
            // TODO: L1 read before L0?
            L1 = reader.ReadByte();
            L0 = reader.ReadByte();
            DttL1 = ReadCoefficients(reader, L1, true);
            DttL0 = ReadCoefficients(reader, L0, false);
            Deserialized = true;
        }

        public override void Write(EndianBinaryWriter writer)
        {
            base.Write(writer);
            writer.Write((byte)DttL0.Length);
            writer.Write((byte)DttL1.Length);
            WriteCoefficients(writer, DttL0);
            WriteCoefficients(writer, DttL1);
        }
    }
}


