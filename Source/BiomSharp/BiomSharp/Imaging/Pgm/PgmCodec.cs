// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.Text;

namespace BiomSharp.Imaging.Pgm
{
    public class PgmCodec : IBitmapCodec, IBitmapCodec<PgmParameters>
    {
        internal class PgmHeader
        {
            public string? Type;
            public List<string> Comments { get; } = new List<string>();
            public int Width;
            public int Height;
            public int Colors;

            public PgmHeader() { }

            public static PgmHeader Read(Stream stream)
            {
                var header = new PgmHeader
                {
                    Type = ReadLine(stream)?[..2]
                };
                if (header.Type is "P5" or "P2")
                {
                    string? line = ReadLine(stream);
                    while (line != null)
                    {
                        if (line.StartsWith("#"))
                        {
                            string comment = line[1..].Trim();
                            if (comment.Length > 0)
                            {
                                header.Comments.Add(comment);
                            }
                        }
                        else
                        {
                            string[] parms = line.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);
                            if (parms.Length == 2)
                            {
                                if (!int.TryParse(parms[0], out header.Width) || header.Width <= 0)
                                {
                                    throw new InvalidOperationException("Width is invalid");
                                }
                                if (!int.TryParse(parms[1], out header.Height) || header.Height <= 0)
                                {
                                    throw new InvalidOperationException("Height is invalid");
                                }
                            }
                            else if (parms.Length == 1)
                            {
                                if (!int.TryParse(parms[0], out header.Colors) || header.Colors != 255)
                                {
                                    throw new InvalidOperationException(
                                        "Colors invalid - must be 8-bit (256 colors)");
                                }
                            }
                        }
                        if (header.Colors == 255 && header.Width > 0 && header.Height > 0)
                        {
                            break;
                        }
                        line = ReadLine(stream);
                    }
                    return header;
                }
                else
                {
                    throw new InvalidOperationException("P2/P5 marker not found");
                }
            }

            private string AsString()
            {
                var sb = new StringBuilder();
                _ = sb.AppendFormat("{0}\n", Type);
                foreach (string comment in Comments)
                {
                    _ = sb.AppendFormat("# {0}\n", comment);
                }
                _ = sb.AppendFormat("{0} {1}\n", Width, Height);
                _ = sb.AppendFormat("{0}\n", Colors);
                return sb.ToString();
            }

            public void Write(Stream stream)
            {
                if ((Type == "P5" || Type == "P2")
                    &&
                    Width > 0 && Height > 0 && Colors == 255)
                {
                    byte[] header = Encoding.ASCII.GetBytes(AsString());
                    stream.Write(header, 0, header.Length);
                }
                else
                {
                    throw new InvalidOperationException("Invalid PGM header");
                }
            }
        };

        private static readonly char[] Whitespace = new char[] { ' ', '\r', '\t' };
        private PgmHeader header;
        private byte[]? pixels;

        public PgmCodec() => header = new PgmHeader();

        private PgmParameters SetReadParameters()
            =>
            new()
            {
                Format = header.Type == "P2" ? PgmFormatType.ASC : PgmFormatType.BIN,
                Comments = header.Comments.Where(c => !string.IsNullOrEmpty(c)).ToArray()
            };

        private void SetWriteParameters(PgmParameters? writeParms)
        {
            if (writeParms != null)
            {
                header.Type = writeParms.Format == PgmFormatType.ASC ? "P2" : "P5";
                header.Colors = 255;
                header.Comments.Clear();
                header.Comments.AddRange(writeParms.Comments.Where(c => !string.IsNullOrEmpty(c)));
            }
        }

        private void ReadPixels(Stream stream)
        {
            pixels = new byte[header.Height * header.Width];
            if (header.Type == "P5")
            {
                _ = stream.Read(pixels, 0, pixels.Length);
            }
            else
            {
                int ro = 0;
                string? line = ReadLine(stream);
                while (line != null)
                {
                    if (line.StartsWith("#"))
                    {
                        string comment = line[1..].Trim();
                        if (comment.Length > 0)
                        {
                            header.Comments.Add(comment);
                        }
                    }
                    else
                    {
                        string[] pix = line.Split(Whitespace, StringSplitOptions.RemoveEmptyEntries);
                        if (pix.Length == header.Width)
                        {
                            for (int x = 0; x < pix.Length; x++)
                            {
                                if (!byte.TryParse(pix[x], out pixels[ro + x]))
                                {
                                    throw new InvalidOperationException(string.Format(
                                        "Invalid pixel value (x, y) = ({0},{1})", x, ro));
                                }
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException(string.Format(
                                "Missing/extra pixels in row y = {0}", ro / header.Width));
                        }
                        ro += header.Width;
                    }
                    line = ReadLine(stream);
                }
            }
        }

        private void WritePixels(Stream stream)
        {
            if (pixels != null
                &&
                pixels.Length == header.Width * header.Height)
            {
                if (header.Type == "P5")
                {
                    stream.Write(pixels, 0, pixels.Length);
                }
                else
                {
                    for (int ro = 0; ro < pixels.Length; ro += header.Width)
                    {
                        var sb = new StringBuilder();
                        for (int x = 0; x < header.Width; x++)
                        {
                            _ = sb.Append(pixels[ro + x]);
                            _ = sb.Append(x + 1 == header.Width ? '\n' : ' ');
                        }
                        byte[] row = Encoding.ASCII.GetBytes(sb.ToString());
                        stream.Write(row, 0, row.Length);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid pixel data");
            }
        }

        private static string? ReadLine(Stream stream)
        {
            var chrs = new List<char>();
            int read = stream.ReadByte();
            while ((char)read != '\n')
            {
                if (read == -1)
                {
                    return null;
                }
                chrs.Add((char)read);
                read = stream.ReadByte();
            }
            return new string(chrs.ToArray());
        }

        #region IBitmapCodec implementation

        public string[]? FileExtensions => new string[] { ".pgm" };

        public void FromRaw(SimpleBitmap? rawImage)
        {
            if (rawImage != null)
            {
                if (rawImage.IsGray)
                {
                    header = new PgmHeader()
                    {
                        Colors = 255,
                        Height = rawImage.Height,
                        Width = rawImage.Width,
                        Type = "P5",
                    };
                    header.Comments.Clear();
                    pixels = (byte[])rawImage.Pixels.Clone();
                }
                else
                {
                    throw new InvalidOperationException("Raw image not 8bpp");
                }
            }
            else
            {
                throw new InvalidOperationException("Raw image is null");
            }
        }

        public SimpleBitmap? ToRaw() => SimpleBitmap.Gray8(
                pixels,
                header.Width,
                header.Height,
                -1);

        public byte[]? Encode()
        {
            using var stream = new MemoryStream();
            Write(stream, new PgmParameters());
            return stream.ToArray();
        }

        public byte[]? Encode<TParms>(TParms? writeParms) where TParms : class, new()
        {
            if (writeParms is PgmParameters pgmWriteParms)
            {
                using var stream = new MemoryStream();
                Write(stream, pgmWriteParms);
                return stream.ToArray();
            }
            throw new PgmCodecException(
                $"{nameof(TParms)} invalid PGM write parameters type");
        }

        public void Decode(byte[] encoded) =>
            Read(new MemoryStream(encoded), out PgmParameters _);

        public void Decode<TParms>(byte[] encoded, out TParms? readParms)
            where TParms : class, new()
            =>
            Read(new MemoryStream(encoded), out readParms);

        public void Read(Stream stream) =>
            Read(stream, out PgmParameters _);

        public void Read<TParms>(Stream stream, out TParms? readParms)
            where TParms : class, new()
        {
            readParms = null;
            if (typeof(TParms).IsAssignableFrom(typeof(PgmParameters)))
            {
                header = PgmHeader.Read(stream);
                ReadPixels(stream);
                readParms = SetReadParameters() as TParms;
            }
            else
            {
                throw new PgmCodecException(
                    $"{nameof(TParms)} is invalid PGM read parameters type");
            }
        }

        public void Write(Stream stream) => Write(stream, new PgmParameters());

        public void Write<TParms>(Stream stream, TParms? writeParms) where TParms : class, new()
        {
            if (writeParms is PgmParameters wsqWriteParms)
            {
                SetWriteParameters(writeParms as PgmParameters);
                header.Write(stream);
                WritePixels(stream);
            }
            else
            {
                throw new PgmCodecException(
                    $"{nameof(TParms)} invalid PGM write parameters type");
            }
        }

        #endregion IBitmapCodec implementation

        #region IBitmapCodec<PgmParameters> implementation

        public byte[]? Encode(PgmParameters? writeParms)
            => Encode<PgmParameters>(writeParms);

        public void Decode(byte[] encoded, out PgmParameters? readParms)
            => Decode<PgmParameters>(encoded, out readParms);

        public void Read(Stream stream, out PgmParameters? readParms)
            => Read<PgmParameters>(stream, out readParms);

        public void Write(Stream stream, PgmParameters? writeParms)
            => Write<PgmParameters>(stream, writeParms);

        #endregion IBitmapCodec<PgmParameters> implementation
    }
}
