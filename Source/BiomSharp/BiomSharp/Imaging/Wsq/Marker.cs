// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Imaging.Wsq.IO;

namespace BiomSharp.Imaging.Wsq
{
    internal enum Marker : ushort
    {
        None = 0,
        SOI = 0xFFA0,
        EOI = 0xFFA1,
        SOF = 0xFFA2,
        SOB = 0xFFA3,
        DTT = 0xFFA4,
        DQT = 0xFFA5,
        DHT = 0xFFA6,
        DRI = 0xFFA7,
        COM = 0xFFA8,
        RST0 = 0xFFB0,
        RST1 = 0xFFB1,
        RST2 = 0xFFB2,
        RST3 = 0xFFB3,
        RST4 = 0xFFB4,
        RST5 = 0xFFB5,
        RST6 = 0xFFB6,
        RST7 = 0xFFB7,
    };

    internal static class MarkerEx
    {
        public static Marker? NextMarker(this EndianBinaryReader reader)
        {
            try
            {
                Marker? marker = null;
                while (marker == null)
                {
                    byte token = reader.ReadByte();
                    while (token != 0xFF)
                    {
                        token = reader.ReadByte();
                    }
                    _ = reader.BaseStream.Seek(-1L, SeekOrigin.Current);
                    marker = reader.ReadMarker();
                    if (marker != null && marker.Value != Marker.None)
                    {
                        break;
                    }

                    marker = null;
                }
                return marker;
            }
            catch (EndOfStreamException)
            {
                return null;
            }
        }

        private static Marker ReadMarker(this EndianBinaryReader reader)
        {
            ushort value = reader.ReadUInt16();
            return
                value is >= ((ushort)Marker.SOI) and <= ((ushort)Marker.COM)
                ?
                (Marker)value : Marker.None;
        }
    }
}
