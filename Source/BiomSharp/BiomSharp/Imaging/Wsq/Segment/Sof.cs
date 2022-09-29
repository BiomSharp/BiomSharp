// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging.Wsq.IO;

namespace BiomSharp.Imaging.Wsq.Segment
{
    // Serialized sizes shown in ()
    internal class Sof : DataSegment
    {
        // SOF: (16 bits) Start of frame marker; marks the beginning of the 
        //      frame parameters.
        public override Marker Marker => Marker.SOF;
        // Lf: (16 bits) Frame header length; specifies the length of the frame header
        // => @base.Size
        // A: (8 bits) scanner black calibration value.
        public byte A { get; private set; }
        // B: (8 bits) scanner white calibration value.
        public byte B { get; private set; }
        // Y: (16 bits) number of lines; specifies the number of lines in the source image.
        public ushort Y { get; private set; }
        // X: (16 bits) number of samples per line; specifies the number of samples 
        //    per line in the source image.
        public ushort X { get; private set; }
        // Em: (8 bits) scale exponent; the decimal point in M is shifted left Em places.
        public byte Em { get; private set; }
        // M: (16 bits) location value for image transformation parameters.
        public ushort M { get; private set; }
        // Er: (8 bits) scale exponent; the decimal point in R is shifted left Er places.
        public byte Er { get; private set; }
        // R: (16 bits) scale value for image transformation parameters.
        public ushort R { get; private set; }
        // Ev: (8 bits) identifies the WSQ encoder algorithm(parameterization) that was 
        //     used on this image. (binary 2 - 0x02 for FBI Encoder Number Two)
        //public sbyte Ev { get; private set; }
        public byte Ev { get; private set; }
        // Sf: (16 bits) identifies the software implementation that encoded this image.
        public short Sf { get; private set; }
        // Shift = M / (10^Em)
        public float Shift { get; private set; }
        // TransformScale = R / (10^Er) 
        public float Scale { get; private set; }

        private Sof()
        {
            A = 0;
            B = 255;
            Ev = 2;
            Sf = Implementer.Id;
        }

        public Sof(
            ushort pixHeight,
            ushort pixWidth,
            float transformShift,
            float transformScale,
            byte blackPixel,
            byte whitePixel,
            //sbyte encodeAlgorithm,
            byte encodeAlgorithm,
            short softwareId)
        {
            Y = pixHeight;
            X = pixWidth;
            Shift = transformShift;
            Scale = transformScale;
            A = blackPixel;
            B = whitePixel;
            //if (encodeAlgorithm != -1)
            if (encodeAlgorithm != 0)
            {
                Ev = encodeAlgorithm;
            }
            //if (softwareId != -1)
            if (softwareId != 0)
            {
                Sf = softwareId;
            }
            // 4 * sizeof(ushort) + sizeof(short) + 4 * sizeof(byte) + sizeof(sbyte)
            ContentSize = 15;
        }

        protected override void Read(EndianBinaryReader reader, Marker marker)
        {
            base.Read(reader, marker);
            A = reader.ReadByte();
            B = reader.ReadByte();
            Y = reader.ReadUInt16();
            X = reader.ReadUInt16();
            Em = reader.ReadByte();
            M = reader.ReadUInt16();
            Shift = Math.FromBasePlusShift(M, Em);
            Er = reader.ReadByte();
            R = reader.ReadUInt16();
            Scale = Math.FromBasePlusShift(R, Er);
            //Ev = reader.ReadSByte();
            Ev = reader.ReadByte();
            Sf = reader.ReadInt16();
            Deserialized = true;
        }

        public override void Write(EndianBinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(A);
            writer.Write(B);
            writer.Write(Y);
            writer.Write(X);
            M = (ushort)Math.ToBasePlusShiftInt(Shift, out int em, out byte _);
            Em = (byte)em;
            writer.Write(Em);
            writer.Write(M);
            R = (ushort)Math.ToBasePlusShiftInt(Scale, out int er, out byte _);
            Er = (byte)er;
            writer.Write(Er);
            writer.Write(R);
            writer.Write(Ev);
            writer.Write(Sf);
        }
    }
}
