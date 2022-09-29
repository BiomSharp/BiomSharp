// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging.Wsq.IO;
using BiomSharp.Imaging.Wsq.Segment;

namespace BiomSharp.Imaging.Wsq
{
    public class WsqCodec : IBitmapCodec, IBitmapCodec<WsqParameters>
    {
        private WsqHeader? nistHeader;
        private readonly List<string> comments = new();

        private SimpleBitmap? decoded;

        public WsqCodec() { }

        private void Encode(EndianBinaryWriter writer, WsqParameters? writeParms)
        {
            if (decoded != null)
            {
                writeParms ??= WsqParameters.Default;
                var encoder = new Encoder(new Segmenter(writer));
                encoder.Encode(
                    decoded,
                    writeParms.NistHeader,
                    writeParms.BitRate,
                    (byte)writeParms.Black,
                    (byte)writeParms.White,
                    writeParms.Filter == WsqFilterType.Odd7x9 ? "7x9" : "8x8",
                    null,
                    null,
                    writeParms.PackedDHT,
                    writeParms.Resolution,
                    writeParms.Comments);
            }
            else
            {
                throw new WsqCodecException("Raw image for encoding is null");
            }
        }

        private void SetReadParameters(Segmenter segmenter, out WsqParameters? parms)
        {
            parms = null;
            Sof sof = segmenter.First<Sof>();
            Dtt dtt = segmenter.First<Dtt>();
            if (nistHeader != null)
            {
                int? itemCount = nistHeader.GetValue<int>(WsqNistConstants.NCM_HEADER);
                if (itemCount is not null and 9)
                {
                    parms = new WsqParameters()
                    {
                        NistHeader = true,
                        BitRate = nistHeader.GetValue<float>(WsqNistConstants.NCM_WSQ_RATE) ?? -1f,
                        Resolution = nistHeader.GetValue<int>(WsqNistConstants.NCM_PPI) ??
                            (decoded?.Resolution != null
                            ? decoded.Resolution >= 100
                            ? decoded.Resolution : -1 : -1),
                        Black = sof.A,
                        White = sof.B,
                        Filter = dtt.L0 == 7 && dtt.L1 == 9
                            ? WsqFilterType.Odd7x9 : WsqFilterType.Even8x8,
                        PackedDHT = segmenter.Segments<Dht>().Count() == 1,
                        Comments = comments.ToArray(),
                        ImplementerId = sof.Sf
                    };
                }
            }
        }

        private void ProcessComments(IEnumerable<Com> segments)
        {
            comments.Clear();
            foreach (Com segment in segments)
            {
                var header = WsqHeader.Deserialize(segment);
                if (header != null)
                {
                    if (nistHeader == null)
                    {
                        nistHeader = header;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "NISTCOM WSQ header already exists - using 1st header");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(segment.Comment))
                    {
                        comments.Add(segment.Comment);
                    }
                }
            }
        }

        #region IBitmapCodec implementation

        public string[]? FileExtensions => new string[] { ".wsq" };

        public void FromRaw(SimpleBitmap? rawImage)
            =>
            decoded =
            rawImage != null
            ? rawImage.IsColor
            ? SimpleBitmap.ToGray(rawImage) : rawImage : null;

        public SimpleBitmap? ToRaw() => decoded?.Clone();

        public byte[]? Encode() => Encode(WsqParameters.Default);

        public byte[]? Encode<TParms>(TParms? writeParms)
            where TParms : class, new()
        {
            if (writeParms is WsqParameters wsqWriteParms)
            {
                using var stream = new MemoryStream();
                Encode(new EndianBinaryWriter(stream), wsqWriteParms);
                return stream.ToArray();
            }
            throw new WsqCodecException(
                $"{nameof(TParms)} invalid WSQ write parameters type");
        }

        public void Decode(byte[] encoded) => Read(new MemoryStream(encoded));

        public void Decode<TParms>(byte[] encoded, out TParms? readParms)
            where TParms : class, new()
            => Read(new MemoryStream(encoded), out readParms);

        public void Read(Stream stream) => Read(stream, out WsqParameters? _);

        public void Read<TParms>(Stream stream, out TParms? readParms)
            where TParms : class, new()
        {
            readParms = null;
            if (typeof(TParms).IsAssignableFrom(typeof(WsqParameters)))
            {
                using var reader = new EndianBinaryReader(stream);
                var segmenter = new Segmenter(reader);
                var decoder = new Decoder(segmenter);
                segmenter.EnumerateSegments();
                ProcessComments(segmenter.Segments<Com>());
                decoded = decoder.Decoded?.Clone();
                if (decoded != null)
                {
                    SetReadParameters(segmenter, out WsqParameters? wsqReadParms);
                    readParms = wsqReadParms as TParms;
                }
                else
                {
                    throw new WsqCodecException("After decoding, decoded image is null");
                }
            }
            else
            {
                throw new WsqCodecException(
                    $"{nameof(TParms)} is invalid WSQ read parameters type");
            }
        }

        public void Write(Stream stream)
            =>
            Encode(new EndianBinaryWriter(stream), WsqParameters.Default);

        public void Write<TParms>(Stream stream, TParms? writeParms)
            where TParms : class, new()
        {
            if (writeParms is WsqParameters wsqWriteParms)
            {
                Encode(new EndianBinaryWriter(stream), wsqWriteParms);
            }
            else
            {
                throw new WsqCodecException(
                    $"{nameof(TParms)} invalid WSQ write parameters type");
            }
        }

        #endregion IBitmapCodec implementation

        #region IBitmapCodec<WsqParameters> implementation

        public byte[]? Encode(WsqParameters? writeParms)
            => Encode<WsqParameters>(writeParms);

        public void Decode(byte[] encoded, out WsqParameters? readParms)
            => Decode<WsqParameters>(encoded, out readParms);

        public void Read(Stream stream, out WsqParameters? readParms)
            => Read<WsqParameters>(stream, out readParms);

        public void Write(Stream stream, WsqParameters? writeParms)
            => Write<WsqParameters>(stream, writeParms);

        #endregion IBitmapCodec<WsqParameters> implementation
    }
}
