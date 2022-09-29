// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging.Wsq.Huffman;
using BiomSharp.Imaging.Wsq.Segment;

namespace BiomSharp.Imaging.Wsq
{
    internal class Decoder
    {
        private Huffman.Decoder? huffmanDecoder;
        private WsqPixelBuffer<int>? huffDecoded;

        public SimpleBitmap? Decoded { get; private set; }


        public Decoder(Segmenter segmenter) => segmenter.SegmentRead += OnSegmentRead;

        private static SimpleBitmap Create8bppRaw(
            float[] img,
            int width,
            int height,
            float shift,
            float scale,
            byte black,
            byte white)
        {
            var rawImage = SimpleBitmap.Gray8(new byte[img.Length], width, height, -1);
            byte[] pixels = rawImage.Pixels;
            for (int idx = 0; idx < img.Length; idx++)
            {
                float pixel = (scale * img[idx]) + shift + 0.5f;
                if (pixel < black)
                {
                    pixels[idx] = black;
                }
                else if (pixel > white)
                {
                    pixels[idx] = white;
                }
                else
                {
                    pixels[idx] = (byte)pixel;
                }
            }
            return rawImage;
        }

        private void OnSegmentRead(Segmenter segmenter, BaseSegment lastRead)
        {
            switch (lastRead.Marker)
            {
                case Marker.SOF:
                    Sof sof = segmenter.First<Sof>();
                    huffmanDecoder = Huffman.Decoder.Create();
                    huffDecoded = new WsqPixelBuffer<int>(sof.X, sof.Y);
                    break;
                case Marker.SOB:
                    DhtTable dhtTable = segmenter.DhtTableWithId(((Sob)lastRead).Td);
                    var hcs = CodeEntries.Create(dhtTable.GetCodeSizes());
                    var hdt = Table.Create(dhtTable, hcs);
                    if (huffmanDecoder != null
                        &&
                        segmenter.Reader != null
                        &&
                        huffDecoded != null)
                    {
                        huffmanDecoder.Decode(segmenter.Reader, huffDecoded, dhtTable, hdt);
                    }
                    else
                    {
                        throw new WsqCodecException(
                            "Huffman decoder, reader or decoded image is null or invalid");
                    }
                    break;
                case Marker.EOI:
                    sof = segmenter.First<Sof>();
                    var quantizer = Quantizer.Create(sof);
                    if (huffDecoded != null)
                    {
                        WsqPixelBuffer<float> fImage = quantizer.Unquantize(huffDecoded, segmenter.First<Dqt>());
                        _ = Transformer.Construct(fImage, segmenter.First<Dtt>(), quantizer);
                        Decoded = Create8bppRaw(
                        fImage.Pixels,
                            fImage.Width,
                            fImage.Height,
                            sof.Shift,
                            sof.Scale,
                            sof.A, sof.B);
                    }
                    else
                    {
                        throw new WsqCodecException(
                            "Huffman decoded image is null or invalid");
                    }
                    break;
                case Marker.None:
                    break;
                case Marker.SOI:
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
