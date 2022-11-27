// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Imaging.Wsq.IO;

namespace BiomSharp.Imaging.Wsq.Segment
{
    // Serialized sizes shown in ()
    internal class DqtElement
    {
        // Eq:  (8 bits) scale exponent; the decimal point in the kth quantization table 
        //      element is moved left Eq places.
        public byte Eq { get; private set; }
        // Q:   (16 bits) quantization table element; specifies the quantization bin 
        //      size for the subband.
        public ushort Q { get; private set; }
        // Ez:  (8 bits) scale exponent; the decimal point in the zero bin quantization table
        //      element is moved left Ez places.
        public byte Ez { get; private set; }
        // Z:   (16 bits) Zero bin table element; specifies the center(zero) bin 
        //      size for the subband.
        public ushort Z { get; private set; }
        // Qf: Normalized coefficient.
        public float Qf { get; private set; }
        // Zf: Normalized coefficient.
        public float Zf { get; private set; }
        // (2 * (sizeof(ushort) + sizeof(byte)))
        public const int SerializeLength = 6;

        public DqtElement() { }

        public void Read(EndianBinaryReader reader)
        {
            Eq = reader.ReadByte();
            Q = reader.ReadUInt16();
            Qf = Math.FromBasePlusShift(Q, Eq);
            Ez = reader.ReadByte();
            Z = reader.ReadUInt16();
            Zf = Math.FromBasePlusShift(Z, Ez);
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(Eq);
            writer.Write(Q);
            writer.Write(Ez);
            writer.Write(Z);
        }

        public static DqtElement CreateRead(EndianBinaryReader reader)
        {
            var filt = new DqtElement();
            filt.Read(reader);
            return filt;
        }

        public DqtElement(float qf, float zf)
        {
            Qf = qf;
            Zf = zf;
            if (Qf != 0F)
            {
                int q = Math.ToBasePlusShiftInt(Qf, out int scale, out byte _);
                if (q > ushort.MaxValue)
                {
                    throw new WsqCodecException("Dqt Q value overflow (> 65535)");
                }
                Q = (ushort)(q & 0xFFFF);
                Eq = (byte)(scale & 0xFF);
                int z = Math.ToBasePlusShiftInt(Zf, out scale, out byte _);
                if (z > ushort.MaxValue)
                {
                    throw new WsqCodecException("Dqt Z value overflow (> 65535)");
                }
                Z = (ushort)(z & 0xFFFF);
                Ez = (byte)(scale & 0xFF);
            }
        }
    }

    internal class Dqt : DataSegment
    {
        public const int MaxSubBands = 64;
        // DQT: (16 bits) define quantization table marker; marks the beginning of the
        //      quantization table specification parameters.
        public override Marker Marker => Marker.DQT;
        // Lq: (16 bits) quantization table definition length; specifies the length of 
        //     all quantization table parameters
        // => @base.Size
        // Ec: (8 bits) scale exponent; the decimal point in C is moved left Ec places.
        public byte Ec { get; private set; }
        // C: (16 bits) quantizer bin center parameter.
        public ushort C { get; private set; }
        // Cf: Normalized  quantizer bin center parameter.
        public float Cf { get; private set; }
        // Q0k: 1..k normalized coefficients Q0..Qk (= @DqtElement.Q)
        public float[] DqtQ { get; private set; }
        // Z1k: 1..k normalized coefficients Z0..Zk (= @DqtElement.Z)
        public float[] DqtZ { get; private set; }
        // Variance
        public float[] Var { get; private set; }
        // Quantization level
        public float Q { get; private set; }
        // Compression ratio
        public float Cr { get; private set; }
        // Compression bitrate
        public float R { get; private set; }

        private Dqt()
        {
            DqtQ = new float[MaxSubBands];
            DqtZ = new float[MaxSubBands];
            Var = new float[MaxSubBands];
            R = 0.75f;
            Ec = 2;
            C = 44;
            // sizeof(byte) + sizeof(ushort) + MaxSubBands * DqtElement.SerializeLength
            ContentSize = 387;
        }

        public Dqt(float bitrate)
            :
            this() => R = bitrate;

        private void ReadElements(EndianBinaryReader reader)
        {
            for (int i = 0; i < MaxSubBands; i++)
            {
                var dqte = DqtElement.CreateRead(reader);
                DqtQ[i] = dqte.Qf;
                DqtZ[i] = dqte.Zf;
            }
        }

        protected override void Read(EndianBinaryReader reader, Marker marker)
        {
            base.Read(reader, marker);
            Ec = reader.ReadByte();
            C = reader.ReadUInt16();
            Cf = Math.FromBasePlusShift(C, Ec);
            ReadElements(reader);
            Deserialized = true;
        }

        private void WriteElements(EndianBinaryWriter writer)
        {
            for (int i = 0; i < MaxSubBands; i++)
            {
                new DqtElement(DqtQ[i], DqtZ[i]).Write(writer);
            }
        }

        public override void Write(EndianBinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(Ec);
            writer.Write(C);
            WriteElements(writer);
        }
    }
}
