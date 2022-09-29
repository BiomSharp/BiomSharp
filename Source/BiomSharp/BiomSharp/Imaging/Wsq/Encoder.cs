// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging.Wsq.Segment;
using BiomSharp.Imaging.Wsq.Tree;

namespace BiomSharp.Imaging.Wsq
{
    internal class Encoder
    {
        public Segmenter? Segmenter { get; private set; }
        private WsqPixelBuffer<short>? quantized;
        private int[]? quantizedSizes;
        private IList<Huffman.Encoder.BlockParms>? blockParms;

        internal Encoder(Segmenter segmenter)
        {
            Segmenter = segmenter;
            Segmenter.SegmentWrite += OnSegmentWrite;
        }

        private static WsqPixelBuffer<float> Create8bppFloat(
            SimpleBitmap rawImage,
            out float shift,
            out float scale,
            byte black,
            byte white)
        {
            var fib = new WsqPixelBuffer<float>(rawImage.Width, rawImage.Height);
            int cnt;
            long sum, ovf;
            float lod, hid;
            sum = 0;
            ovf = 0;
            int low = white;
            int high = black;
            byte[] pixels = rawImage.Pixels;

            for (cnt = 0; cnt < rawImage.Pixels.Length; cnt++)
            {
                int raw = pixels[cnt] & 0xFF;
                if (raw > high)
                {
                    high = raw;
                }
                if (raw < low)
                {
                    low = raw;
                }
                sum += raw;
                if (sum < ovf)
                {
                    throw new WsqCodecException(
                        "Image data byte-to-float overflow, input too big");
                }
                ovf = sum;
            }
            shift = (float)sum / pixels.Length;
            lod = shift - low;
            hid = high - shift;
            if (lod >= hid)
            {
                scale = lod;
            }
            else
            {
                scale = hid;
            }
            scale /= 128f;
            float[] fip = fib.Pixels;
            for (cnt = 0; cnt < pixels.Length; cnt++)
            {
                fip[cnt] = ((pixels[cnt] & 0xFF) - shift) / scale;
            }
            return fib;
        }

        public void Encode(
            SimpleBitmap rawImage,
            bool writeNistHeader,
            float bitrate,
            byte black,
            byte white,
            string filterName,
            float[]? dttL0,
            float[]? dttL1,
            bool packedDht,
            int resolution = -1,
            IEnumerable<string>? comments = null)
        {
            if (rawImage != null)
            {
                if (Segmenter != null)
                {
                    Filter filter = Filter.Odd7x9;
                    if (!string.IsNullOrEmpty(filterName))
                    {
                        if (filterName.ToLower() == "7x9")
                        {
                            filter = Filter.Odd7x9;
                        }
                        else if (filterName.ToLower() == "8x8")
                        {
                            filter = Filter.Even8x8;
                        }
                        else
                        {
                            throw new WsqCodecException(
                                $"Built-in DTT filter '{filterName}' not defined");
                        }
                    }
                    else if (dttL1 != null && dttL1.Length > 0 &&
                        dttL0 != null && dttL0.Length > 0)
                    {
                        filter = Filter.Create(dttL0, dttL1);
                    }
                    if (rawImage.Width > ushort.MaxValue || rawImage.Height > ushort.MaxValue)
                    {
                        throw new WsqCodecException("Image width/height > 65535 pixels");
                    }
                    Segmenter.AddWriteSegment(BaseSegment.CreateInstance(Marker.SOI));
                    if (writeNistHeader)
                    {
                        var wsqHeader = new WsqHeader(
                            rawImage.Width,
                            rawImage.Height,
                            8,
                            rawImage.Resolution > 0 ? rawImage.Resolution : resolution > 0 ? resolution : -1,
                            bitrate);
                        Segmenter.AddWriteSegment(wsqHeader.Serialize());
                    }
                    if (comments != null && comments.Any())
                    {
                        foreach (string comment in comments)
                        {
                            Segmenter.AddWriteSegment(new Com(comment));
                        }
                    }
                    Segmenter.AddWriteSegment(new Dtt(filter));
                    var quantizer = Quantizer.Create(rawImage.Width, rawImage.Height);
                    WsqPixelBuffer<float> fImage =
                        Create8bppFloat(rawImage, out float shift, out float scale, 0, 255);
                    _ = Transformer.Decompose(fImage, quantizer, filter);
                    var dqt = new Dqt(bitrate);
                    Segmenter.AddWriteSegment(dqt);
                    quantized = quantizer.Quantize(out int[] quantSizes, fImage, dqt);
                    quantizedSizes = quantSizes;
                    var sof = new Sof(
                        (ushort)rawImage.Height, (ushort)rawImage.Width,
                        shift, scale, black, white, 2, Implementer.Id);
                    Segmenter.AddWriteSegment(sof);
                    _ = Huffman.Encoder.Create();
                    blockParms = Huffman.Encoder.Encode(quantized, quantSizes);
                    if (blockParms.Count != 2)
                    {
                        throw new WsqCodecException("Huffman block parms count invalid");
                    }
                    if (packedDht)
                    {
                        var dht = new Dht(0, blockParms[0].Bits, blockParms[0].Values);
                        Segmenter.AddWriteSegment(dht);
                        Segmenter.AddWriteSegment(new Sob(0));
                        dht.AddTable(new DhtTable(1, blockParms[1].Bits, blockParms[1].Values));
                        Segmenter.AddWriteSegment(new Sob(1));
                    }
                    else
                    {
                        Segmenter.AddWriteSegment(new Dht(0, blockParms[0].Bits, blockParms[0].Values));
                        Segmenter.AddWriteSegment(new Sob(0));
                        Segmenter.AddWriteSegment(new Dht(1, blockParms[1].Bits, blockParms[1].Values));
                        Segmenter.AddWriteSegment(new Sob(1));
                    }
                    Segmenter.AddWriteSegment(new Sob(1));
                    Segmenter.AddWriteSegment(BaseSegment.CreateInstance(Marker.EOI));
                    Segmenter.WriteAll();
                }
                else
                {
                    throw new WsqCodecException("Encode segmenter is null");
                }
            }
            else
            {
                throw new WsqCodecException("Raw image is null");
            }
        }

        private void WriteBlock(int index)
        {
            if (Segmenter != null && Segmenter.Writer != null)
            {
                if (quantized != null
                    &&
                    quantizedSizes != null && quantizedSizes.Length >= 3
                    &&
                    blockParms != null && blockParms.Count >= 2)
                {
                    if (index == 0)
                    {
                        Huffman.Encoder.CompressBlock(
                            Segmenter.Writer, quantized, 0, quantizedSizes[0], blockParms[0].Codes);
                    }
                    else if (index == 1)
                    {
                        Huffman.Encoder.CompressBlock(
                            Segmenter.Writer, quantized, quantizedSizes[0], quantizedSizes[1],
                            blockParms[1].Codes);
                    }
                    else
                    {
                        Huffman.Encoder.CompressBlock(
                            Segmenter.Writer, quantized, quantizedSizes[0] + quantizedSizes[1],
                            quantizedSizes[2], blockParms[1].Codes);
                    }
                }
                else
                {
                    throw new WsqCodecException(
                        "Encode quantizer is null or invalid");
                }
            }
            else
            {
                throw new WsqCodecException(
                    "Encode segmenter/writer is null or invalid");
            }
        }

        private void OnSegmentWrite(Segmenter segmenter, BaseSegment lastWrite)
        {
            switch (lastWrite.Marker)
            {
                case Marker.SOB:
                    int index = segmenter.IndexOf((Sob)lastWrite);
                    WriteBlock(index);
                    break;
                case Marker.None:
                    break;
                case Marker.SOI:
                    break;
                case Marker.EOI:
                    break;
                case Marker.SOF:
                    break;
                case Marker.DTT:
                    break;
                case Marker.DQT:
                    break;
                case Marker.DHT:
                    break;
                case Marker.DRI:
                    break;
                case Marker.COM:
                    break;
                case Marker.RST0:
                    break;
                case Marker.RST1:
                    break;
                case Marker.RST2:
                    break;
                case Marker.RST3:
                    break;
                case Marker.RST4:
                    break;
                case Marker.RST5:
                    break;
                case Marker.RST6:
                    break;
                case Marker.RST7:
                    break;
                default:
                    break;
            }
        }
    }
}
