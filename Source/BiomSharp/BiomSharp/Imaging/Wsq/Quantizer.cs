// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using BiomSharp.Imaging.Wsq.Segment;
using BiomSharp.Imaging.Wsq.Tree;

namespace BiomSharp.Imaging.Wsq
{
    internal class Quantizer
    {
        public const int StartSubband2 = 19;
        public const int StartSubband3 = 52;
        public const int SubbandCount = 60;
        public const int StartSizeRegion2 = 4;
        public const int StartSizeRegion3 = 51;
        public const float VarThreshold = 1.01F;
        public WTree? WTree { get; private set; }
        public QTree? QTree { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        private Quantizer() { }

        public static Quantizer Create(Sof sof)
        {
            var quantizer = new Quantizer
            {
                Width = sof.X,
                Height = sof.Y,
                WTree = new WTree(sof.X, sof.Y)
            };
            quantizer.QTree = new QTree(quantizer.WTree);
            return quantizer;
        }

        public static Quantizer Create(int width, int height)
        {
            var quantizer = new Quantizer
            {
                WTree = new WTree(width, height)
            };
            quantizer.QTree = new QTree(quantizer.WTree);
            return quantizer;
        }

        private void SetVariance(Dqt dqt, WsqPixelBuffer<float> image)
        {
            float[] pixels = image.Pixels;
            int fpIdx;
            int lenx, leny;
            int skipx, skipy;
            int row, col;
            float ssq;
            float sum2;
            float sumPix;
            float vsum = 0F;

            if (QTree != null)
            {
                for (int cvr = 0; cvr < 4; cvr++)
                {
                    fpIdx = (QTree[cvr].Y * image.Width) + QTree[cvr].X;
                    ssq = 0F;
                    sumPix = 0F;

                    skipx = QTree[cvr].LenX / 8;
                    skipy = 9 * QTree[cvr].LenY / 32;

                    lenx = 3 * QTree[cvr].LenX / 4;
                    leny = 7 * QTree[cvr].LenY / 16;

                    fpIdx += (skipy * image.Width) + skipx;
                    for (row = 0; row < leny; row++, fpIdx += image.Width - lenx)
                    {
                        for (col = 0; col < lenx; col++)
                        {
                            sumPix += pixels[fpIdx];
                            ssq += pixels[fpIdx] * pixels[fpIdx];
                            fpIdx++;
                        }
                    }
                    sum2 = sumPix * sumPix / (lenx * leny);
                    dqt.Var[cvr] = (ssq - sum2) / ((lenx * leny) - 1F);
                    vsum += dqt.Var[cvr];
                }

                if (vsum < 20000F)
                {
                    for (int cvr = 0; cvr < SubbandCount; cvr++)
                    {
                        fpIdx = (QTree[cvr].Y * image.Width) + QTree[cvr].X;
                        ssq = 0F;
                        sumPix = 0F;

                        lenx = QTree[cvr].LenX;
                        leny = QTree[cvr].LenY;

                        for (row = 0; row < leny; row++, fpIdx += image.Width - lenx)
                        {
                            for (col = 0; col < lenx; col++)
                            {
                                sumPix += pixels[fpIdx];
                                ssq += pixels[fpIdx] * pixels[fpIdx];
                                fpIdx++;
                            }
                        }
                        sum2 = sumPix * sumPix / (lenx * leny);
                        dqt.Var[cvr] = (ssq - sum2) / ((lenx * leny) - 1F);
                    }
                }
                else
                {
                    for (int cvr = 4; cvr < SubbandCount; cvr++)
                    {
                        fpIdx = (QTree[cvr].Y * image.Width) + QTree[cvr].X;
                        ssq = 0F;
                        sumPix = 0F;

                        skipx = QTree[cvr].LenX / 8;
                        skipy = 9 * QTree[cvr].LenY / 32;

                        lenx = 3 * QTree[cvr].LenX / 4;
                        leny = 7 * QTree[cvr].LenY / 16;

                        fpIdx += (skipy * image.Width) + skipx;
                        for (row = 0; row < leny; row++, fpIdx += image.Width - lenx)
                        {
                            for (col = 0; col < lenx; col++)
                            {
                                sumPix += pixels[fpIdx];
                                ssq += pixels[fpIdx] * pixels[fpIdx];
                                fpIdx++;
                            }
                        }
                        sum2 = sumPix * sumPix / (lenx * leny);
                        dqt.Var[cvr] = (ssq - sum2) / ((lenx * leny) - 1F);
                    }
                }
            }
            else
            {
                throw new WsqCodecException("QTree is null or invalid");
            }
        }

        private WsqPixelBuffer<short> QuantizeImage(out int[] quantSizes, WsqPixelBuffer<float> fImage, Dqt dqt)
        {
            if (QTree != null && WTree != null)
            {
                float[] a = new float[SubbandCount];
                for (int i = 0; i < StartSubband3; i++)
                {
                    a[i] = 1F;
                }

                a[StartSubband3] = 1.32F;
                a[StartSubband3 + 1] = 1.08F;
                a[StartSubband3 + 2] = 1.42F;
                a[StartSubband3 + 3] = 1.08F;
                a[StartSubband3 + 4] = 1.32F;
                a[StartSubband3 + 5] = 1.42F;
                a[StartSubband3 + 6] = 1.08F;
                a[StartSubband3 + 7] = 1.08F;

                for (int cnt = 0; cnt < SubbandCount; cnt++)
                {
                    if (dqt.Var[cnt] < VarThreshold)
                    {
                        dqt.DqtQ[cnt] = 0F;
                    }
                    else
                    {
                        if (cnt < StartSizeRegion2)
                        {
                            dqt.DqtQ[cnt] = 1F;
                        }
                        else
                        {
                            dqt.DqtQ[cnt] = 10F / (a[cnt] * (float)System.Math.Log(dqt.Var[cnt]));
                        }
                    }
                }

                float[] m = new float[SubbandCount];
                float m1 = 1F / 1024F;
                float m2 = 1F / 256F;
                float m3 = 1F / 16F;
                for (int i = 0; i < StartSizeRegion2; i++)
                {
                    m[i] = m1;
                }

                for (int i = StartSizeRegion2; i < SubbandCount; i++)
                {
                    m[i] = m2;
                }

                for (int i = StartSizeRegion3; i < SubbandCount; i++)
                {
                    m[i] = m3;
                }

                int k0Len = 0;
                int[] k0 = new int[SubbandCount];
                int[] k1 = new int[SubbandCount];
                float[] sigma = new float[SubbandCount];
                for (int cnt = 0; cnt < SubbandCount; cnt++)
                {
                    if (dqt.Var[cnt] >= VarThreshold)
                    {
                        k0[k0Len] = cnt;
                        k1[k0Len++] = cnt;
                        sigma[cnt] = (float)System.Math.Sqrt(dqt.Var[cnt]);
                    }
                }

                int nkIndex;
                int kIndex = 0;
                int kLen = k0Len;
                float q;
                while (true)
                {
                    float s = 0F;
                    for (int i = 0; i < kLen; i++)
                    {
                        s += m[k1[kIndex + i]];
                    }
                    float p = 1F;
                    for (int i = 0; i < kLen; i++)
                    {
                        p *= (float)System.Math.Pow(
                            sigma[k1[kIndex + i]] / dqt.DqtQ[k1[kIndex + i]], m[k1[kIndex + i]]);
                    }
                    q = (float)System.Math.Pow(
                        2, (dqt.R / s) - 1F) / 2.5F / (float)System.Math.Pow(p, 1F / s);
                    bool[] nonPos = new bool[SubbandCount];
                    int npLen = 0;
                    for (int i = 0; i < kLen; i++)
                    {
                        if (dqt.DqtQ[k1[kIndex + i]] / q >= 5F * sigma[k1[kIndex + i]])
                        {
                            nonPos[k1[kIndex + i]] = true;
                            npLen++;
                        }
                    }

                    if (npLen == 0)
                    {
                        break;
                    }

                    nkIndex = 0;
                    int nkLen = 0;
                    for (int i = 0; i < kLen; i++)
                    {
                        if (!nonPos[k1[kIndex + i]])
                        {
                            k1[nkIndex + nkLen++] = k1[kIndex + i];
                        }
                    }

                    kIndex = nkIndex;
                    kLen = nkLen;
                }

                nkIndex = 0;

                for (int i = nkIndex; i < SubbandCount; i++)
                {
                    k1[i] = 0;
                }

                for (int i = 0; i < k0Len; i++)
                {
                    k1[nkIndex + k0[i]] = 1;
                }
                for (int cnt = 0; cnt < SubbandCount; cnt++)
                {
                    if (k1[nkIndex + cnt] != 0)
                    {
                        dqt.DqtQ[cnt] /= q;
                    }
                    else
                    {
                        dqt.DqtQ[cnt] = 0F;
                    }
                    dqt.DqtZ[cnt] = 1.2F * dqt.DqtQ[cnt];
                }

                var sImage = new WsqPixelBuffer<short>(fImage.Width, fImage.Height);
                short[] spix = sImage.Pixels;
                int sIdx = 0;
                float[] fpix = fImage.Pixels;
                int row, col;
                float zbin;
                for (int cnt = 0; cnt < SubbandCount; cnt++)
                {
                    int fIdx = (QTree[cnt].Y * fImage.Width) + QTree[cnt].X;

                    if (dqt.DqtQ[cnt] != 0F)
                    {

                        zbin = dqt.DqtZ[cnt] / 2F;

                        for (row = 0; row < QTree[cnt].LenY; row++, fIdx += fImage.Width - QTree[cnt].LenX)
                        {
                            for (col = 0; col < QTree[cnt].LenX; col++)
                            {
                                if (-zbin <= fpix[fIdx] && fpix[fIdx] <= zbin)
                                {
                                    spix[sIdx] = 0;
                                }
                                else if (fpix[fIdx] > 0F)
                                {
                                    spix[sIdx] = (short)(((fpix[fIdx] - zbin) / dqt.DqtQ[cnt]) + 1F);
                                }
                                else
                                {
                                    spix[sIdx] = (short)(((fpix[fIdx] + zbin) / dqt.DqtQ[cnt]) - 1F);
                                }
                                sIdx++;
                                fIdx++;
                            }
                        }
                    }
                }

                int qSize = sIdx;
                quantSizes = new int[3]
                {
                WTree[14].LenX * WTree[14].LenY,
                (WTree[5].LenY * WTree[1].LenX) + (WTree[4].LenX * WTree[4].LenY),
                (WTree[2].LenX * WTree[2].LenY) + (WTree[3].LenX * WTree[3].LenY),
                };
                for (int node = 0; node < StartSubband2; node++)
                {
                    if (dqt.DqtQ[node] == 0F)
                    {
                        quantSizes[0] -= QTree[node].LenX * QTree[node].LenY;
                    }
                }
                for (int node = StartSubband2; node < StartSubband3; node++)
                {
                    if (dqt.DqtQ[node] == 0F)
                    {
                        quantSizes[1] -= QTree[node].LenX * QTree[node].LenY;
                    }
                }
                for (int node = StartSubband3; node < SubbandCount; node++)
                {
                    if (dqt.DqtQ[node] == 0F)
                    {
                        quantSizes[2] -= QTree[node].LenX * QTree[node].LenY;
                    }
                }
                if (qSize != quantSizes[0] + quantSizes[1] + quantSizes[2])
                {
                    throw new WsqCodecException("Quantization block sizes invalid");
                }
                return sImage;
            }
            else
            {
                throw new WsqCodecException("QTree/WTree is null or invalid");
            }
        }

        public WsqPixelBuffer<float> Unquantize(WsqPixelBuffer<int> huffDecoded, Dqt dqt)
        {
            if (QTree != null)
            {
                int ip = 0;
                var fImage = new WsqPixelBuffer<float>(Width, Height);
                float[] pix = fImage.Pixels;
                int[] hdc = huffDecoded.Pixels;

                for (int cnt = 0; cnt < SubbandCount; cnt++)
                {
                    if (dqt.DqtQ[cnt] != 0F)
                    {
                        int fp = (QTree[cnt].Y * fImage.Width) + QTree[cnt].X;
                        for (int row = 0; row < QTree[cnt].LenY; row++, fp += fImage.Width - QTree[cnt].LenX)
                        {
                            for (int col = 0; col < QTree[cnt].LenX; col++)
                            {
                                if (hdc[ip] == 0)
                                {
                                    pix[fp] = 0f;
                                }
                                else if (hdc[ip] > 0)
                                {
                                    pix[fp] = (dqt.DqtQ[cnt] * (hdc[ip] - dqt.Cf)) + (dqt.DqtZ[cnt] / 2F);
                                }
                                else if (hdc[ip] < 0)
                                {
                                    pix[fp] = (dqt.DqtQ[cnt] * (hdc[ip] + dqt.Cf)) - (dqt.DqtZ[cnt] / 2F);
                                }
                                else
                                {
                                    throw new WsqCodecException("Invalid quantization pixel value");
                                }
                                ++fp;
                                ++ip;
                            }
                        }
                    }
                }
                return fImage;
            }
            else
            {
                throw new WsqCodecException("QTree is null or invalid");
            }
        }

        public WsqPixelBuffer<short> Quantize(out int[] quantSizes, WsqPixelBuffer<float> fImage, Dqt dqt)
        {
            SetVariance(dqt, fImage);
            WsqPixelBuffer<short> sImage = QuantizeImage(out quantSizes, fImage, dqt);
            return sImage;
        }
    }
}
