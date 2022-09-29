// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Net;
using System.Text;
using BiomSharp.Extensions;
using BiomSharp.Nist;
using BiomSharp.Primitives;

namespace BiomSharp.Biometrics.Hand.Serialization
{
    public static class MinutiaeISOSerializer
    {
        internal static void Serialize(this HandMinutiae minutiae, Stream stream)
        {
            using var memStream = new MemoryStream();
            using var writer = new BinaryWriter(memStream, Encoding.UTF8);

            // 4B magic "FMR\0"
            writer.Write("FMR\0".ToCharArray());
            // 4B version (ignored, set to " 20\0"
            writer.Write(" 20\0".ToCharArray());
            // 4B total length (including header, will be updated later)
            writer.Write(0);
            // 2B capture equipment (zeroed)
            writer.Write((short)0);
            // 2B image size in pixels X
            writer.Write(IPAddress.HostToNetworkOrder((short)minutiae.SourceBounds.Width));
            // 2B image size in pixels Y
            writer.Write(IPAddress.HostToNetworkOrder((short)minutiae.SourceBounds.Height));
            // Assume resolution X == Y
            // 2B horizontal resolution (pixels per cm X)
            int resolution = minutiae.Resolution.ToPerCm();
            writer.Write(IPAddress.HostToNetworkOrder((short)resolution));
            // 2B vertical resolution (pixels per cm Y)
            writer.Write(IPAddress.HostToNetworkOrder((short)resolution));
            // 1B rubbish (number of fingerprints, set to 1)
            writer.Write((byte)1);
            // 1B reserved (zeroed)
            writer.Write((byte)0);
            // 1B finger position
            writer.Write((byte)minutiae.FingerCode);
            //  1B view number (0xF0); impression type (0x0F) (zeroed)
            writer.Write((byte)0);
            // 1B fingerprint quality
            writer.Write((byte)minutiae.FingerCode);
            // 1B minutia count
            AssertException.Check(minutiae.FeatureCount < 256, "Fingerprint minutiae count >= 256");
            writer.Write((byte)minutiae.FeatureCount);
            // N*6B minutiae
            for (int i = 0; i < minutiae.FeatureCount; i++)
            {
                IHandMinutia minutia = minutiae.Features[i];
                // 2B minutia position X in pixels
                // 2b (upper) minutia type 
                // (01 ending, 10 bifurcation, 00 other (considered ending))
                int x = minutia.X;
                AssertException.Check(x <= 0x3fff, "X position is out of range");
                int type = minutia.Type switch
                {
                    HandMinutiaType.RidgeEnd => 0x4000,
                    HandMinutiaType.Bifurcation => 0x8000,
                    HandMinutiaType.Unknown => throw new NotImplementedException(),
                    _ => 0,
                };
                writer.Write(IPAddress.HostToNetworkOrder(unchecked((short)(x | type))));
                // 2B minutia position Y in pixels (upper 2b ignored, zeroed)
                int y = minutiae.SourceBounds.Height - minutia.Y;
                AssertException.Check(y <= 0x3fff, "Y position is out of range");
                writer.Write(IPAddress.HostToNetworkOrder((short)y));
                // 1B direction
                int t = minutia.Theta + 180;
                if (t >= 360)
                {
                    t -= 360;
                }

                t = (int)(((double)t * 256 / 360) + 0.5);
                AssertException.Check(t < 256, "T angle is out of range");
                writer.Write((byte)t);
                // 1B quality
                byte quality = (byte)(minutia.Quality != null ? minutia.Quality : 0);
                AssertException.Check(quality <= 100, "Quality is out of range.");
                writer.Write(quality);
            }
            // 2B extra data length, zeroed
            // N*1B extra data
            writer.Write((short)0);
            byte[] buffer = memStream.ToArray();
            byte[] bytes = new byte[buffer.Length];
            Buffer.BlockCopy(buffer, 0, bytes, 0, bytes.Length);
            BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)buffer.Length)).CopyTo(bytes, 10);
            stream.Write(bytes, 0, bytes.Length);
        }


        internal static HandMinutiae Deserialize(this Stream stream)
        {
            using var reader = new BinaryReader(stream);

            reader.BaseStream.Position = 0L;
            // 4B magic "FMR\0"
            AssertException.Check(
                new string(reader.ReadChars(4)) == "FMR\0", "This is not an ISO template.");
            // 4B version (ignored, set to " 20\0")
            _ = reader.ReadChars(4);
            // 4B total length (including header)
            AssertException.Check(
                IPAddress.NetworkToHostOrder(
                    reader.ReadInt32()) == stream.Length, "Invalid template length.");
            // 2B capture equipment (zeroed)
            _ = reader.ReadInt16();
            // 2B image size in pixels X
            int imageWidth = IPAddress.NetworkToHostOrder(reader.ReadInt16());
            // 2B image size in pixels Y
            int imageHeight = IPAddress.NetworkToHostOrder(reader.ReadInt16());
            var sourceBounds = new Size(imageWidth, imageHeight);
            // 2B horizontal resolution (pixels per cm X)
            int horzRes = IPAddress.NetworkToHostOrder(reader.ReadInt16());
            // 2B vertical resolution (pixels per cm Y)
            int vertRes = IPAddress.NetworkToHostOrder(reader.ReadInt16());
            AssertException.Check(horzRes != vertRes,
                "ISO template vertical != horizontal resolution");
            List<HandMinutia> minutiae = new();
            // 1B number of fingerprints (set to 1)
            AssertException.Check(
                reader.ReadByte() == 1,
                "Only single-fingerprint ISO templates are supported.");
            // 1B reserved (zeroed)
            _ = reader.ReadByte();
            // 1B finger position
            var fingerCode = (NistFingerCode)reader.ReadByte();
            // 1B view number (0xF0); impression type (0x0F) (zeroed)
            var imprint = (NistFingerImprint)(reader.ReadByte() & 0x0F);
            // 1B fingerprint quality
            int isoQuality = reader.ReadByte();
            // 1B minutia count
            int minutiaCount = reader.ReadByte();
            // N*6B minutiae
            for (int i = 0; i < minutiaCount; i++)
            {
                var minutia = new HandMinutia();
                // 2B minutia position X in pixels
                // 2B (upper) minutia type (01 ending, 10 bifurcation, 00 other (considered ending))
                ushort xPacked = (ushort)IPAddress.NetworkToHostOrder(reader.ReadInt16());
                minutia.X = xPacked & 0x3fff;
                minutia.Type = (xPacked & 0xc000) switch
                {
                    0x4000 => HandMinutiaType.RidgeEnd,
                    0x8000 => HandMinutiaType.Bifurcation,
                    _ => HandMinutiaType.Unknown,
                };
                // 2B minutia position Y in pixels (upper 2b ignored, zeroed)
                minutia.Y = (imageHeight -
                    (ushort)IPAddress.NetworkToHostOrder(reader.ReadInt16())) & 0x3fff;
                // 1B direction
                int t = (int)((double)reader.ReadByte() - 128);
                if (t < 0)
                {
                    t += 256;
                }

                minutia.Theta = (int)(((double)t * 360 / 256) + 0.5);
                // 1B quality (ignored, zeroed)
                minutia.Quality = reader.ReadByte();
                AssertException.Check(minutia.Quality <= 100,
                    "Minutia quality is out of range.");
                minutiae.Add(minutia);
            }
            // 2B extra data length, ignored
            // N*1B extra data, ignored
            return new HandMinutiae(
                fingerCode, imprint, horzRes, minutiae,
                new Rectangle(Point.Empty, sourceBounds),
                isoQuality);
        }
    }
}
