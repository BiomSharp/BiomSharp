// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging.Wsq.Segment;
using BiomSharp.Imaging.Wsq.Tree;

namespace BiomSharp.Imaging.Wsq
{
    internal class Transformer
    {
        private Transformer() { }

        private static void Reconstruct(WsqPixelBuffer<float> image, Dtt dtt, Quantizer quantizer)
        {
            if (quantizer.WTree != null)
            {
                float[] temp = new float[image.Length];
                WTree wtree = quantizer.WTree;
                for (int node = WTree.Length - 1; node >= 0; node--)
                {
                    int idx = (wtree[node].Y * image.Width) + wtree[node].X;
                    JoinLets(temp, image.Pixels, 0, idx,
                        wtree[node].LenX, wtree[node].LenY,
                        1, image.Width,
                        dtt.DttL1, dtt.L1,
                        dtt.DttL0, dtt.L0,
                        wtree[node].InvC);
                    JoinLets(image.Pixels, temp, idx, 0,
                        wtree[node].LenY, wtree[node].LenX,
                        image.Width, 1,
                        dtt.DttL1, dtt.L1,
                        dtt.DttL0, dtt.L0,
                        wtree[node].InvR);
                }
            }
            else
            {
                throw new WsqCodecException("Quantizer WTree is null or invalid");
            }
        }

        private static void JoinLets(
        float[] newdata,
        float[] olddata,
        int newIndex,
        int oldIndex,
        int len1,
        int len2,
        int pitch,
        int stride,
        float[] hi,
        int hsz,
        float[] lo,
        int lsz,
        int inv)
        {
            int lp0, lp1;
            int hp0, hp1;
            int lopass, hipass;
            int limg, himg;
            int pix, cl_rw;
            int i, da_ev;
            int loc, hoc;
            int hlen, llen;
            int nstr, pstr;
            int tap;
            int fi_ev;
            int olle, ohle, olre, ohre;
            int lle, lle2, lre, lre2;
            int hle, hle2, hre, hre2;
            int lpx, lspx;
            int lpxstr, lspxstr;
            int lstap, lotap;
            int hpx, hspx;
            int hpxstr, hspxstr;
            int hstap, hotap;
            int asym, fhre = 0, ofhre;
            float ssfac, osfac, sfac;

            da_ev = len2 % 2;
            fi_ev = lsz % 2;
            pstr = stride;
            nstr = -pstr;
            if (da_ev != 0)
            {
                llen = (len2 + 1) / 2;
                hlen = llen - 1;
            }
            else
            {
                llen = len2 / 2;
                hlen = llen;
            }

            if (fi_ev != 0)
            {
                asym = 0;
                ssfac = 1F;
                ofhre = 0;
                loc = (lsz - 1) / 4;
                hoc = ((hsz + 1) / 4) - 1;
                lotap = (lsz - 1) / 2 % 2;
                hotap = (hsz + 1) / 2 % 2;
                if (da_ev != 0)
                {
                    olle = 0;
                    olre = 0;
                    ohle = 1;
                    ohre = 1;
                }
                else
                {
                    olle = 0;
                    olre = 1;
                    ohle = 1;
                    ohre = 0;
                }
            }
            else
            {
                asym = 1;
                ssfac = -1F;
                ofhre = 2;
                loc = (lsz / 4) - 1;
                hoc = (hsz / 4) - 1;
                lotap = lsz / 2 % 2;
                hotap = hsz / 2 % 2;
                if (da_ev != 0)
                {
                    olle = 1;
                    olre = 0;
                    ohle = 1;
                    ohre = 1;
                }
                else
                {
                    olle = 1;
                    olre = 1;
                    ohle = 1;
                    ohre = 1;
                }

                if (loc == -1)
                {
                    loc = 0;
                    olle = 0;
                }
                if (hoc == -1)
                {
                    hoc = 0;
                    ohle = 0;
                }
                for (i = 0; i < hsz; i++)
                {
                    hi[i] = -hi[i];
                }
            }

            for (cl_rw = 0; cl_rw < len1; cl_rw++)
            {
                limg = newIndex + (cl_rw * pitch);
                himg = limg;
                newdata[himg] = 0F;
                newdata[himg + stride] = 0F;
                if (inv != 0)
                {
                    hipass = oldIndex + (cl_rw * pitch);
                    lopass = hipass + (stride * hlen);
                }
                else
                {
                    lopass = oldIndex + (cl_rw * pitch);
                    hipass = lopass + (stride * llen);
                }


                lp0 = lopass;
                lp1 = lp0 + ((llen - 1) * stride);
                lspx = lp0 + (loc * stride);
                lspxstr = nstr;
                lstap = lotap;
                lle2 = olle;
                lre2 = olre;

                hp0 = hipass;
                hp1 = hp0 + ((hlen - 1) * stride);
                hspx = hp0 + (hoc * stride);
                hspxstr = nstr;
                hstap = hotap;
                hle2 = ohle;
                hre2 = ohre;
                osfac = ssfac;

                for (pix = 0; pix < hlen; pix++)
                {
                    for (tap = lstap; tap >= 0; tap--)
                    {
                        lle = lle2;
                        lre = lre2;
                        lpx = lspx;
                        lpxstr = lspxstr;

                        newdata[limg] = olddata[lpx] * lo[tap];
                        for (i = tap + 2; i < lsz; i += 2)
                        {
                            if (lpx == lp0)
                            {
                                if (lle != 0)
                                {
                                    lpxstr = 0;
                                    lle = 0;
                                }
                                else
                                {
                                    lpxstr = pstr;
                                }
                            }
                            if (lpx == lp1)
                            {
                                if (lre != 0)
                                {
                                    lpxstr = 0;
                                    lre = 0;
                                }
                                else
                                {
                                    lpxstr = nstr;
                                }
                            }
                            lpx += lpxstr;
                            newdata[limg] += olddata[lpx] * lo[i];
                        }
                        limg += stride;
                    }
                    if (lspx == lp0)
                    {
                        if (lle2 != 0)
                        {
                            lspxstr = 0;
                            lle2 = 0;
                        }
                        else
                        {
                            lspxstr = pstr;
                        }
                    }
                    lspx += lspxstr;
                    lstap = 1;

                    for (tap = hstap; tap >= 0; tap--)
                    {
                        hle = hle2;
                        hre = hre2;
                        hpx = hspx;
                        hpxstr = hspxstr;
                        fhre = ofhre;
                        sfac = osfac;

                        for (i = tap; i < hsz; i += 2)
                        {
                            if (hpx == hp0)
                            {
                                if (hle != 0)
                                {
                                    hpxstr = 0;
                                    hle = 0;
                                }
                                else
                                {
                                    hpxstr = pstr;
                                    sfac = 1.0f;
                                }
                            }
                            if (hpx == hp1)
                            {
                                if (hre != 0)
                                {
                                    hpxstr = 0;
                                    hre = 0;
                                    if (asym != 0 && da_ev != 0)
                                    {
                                        hre = 1;
                                        fhre--;
                                        sfac = fhre;
                                        if (sfac == 0.0)
                                        {
                                            hre = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    hpxstr = nstr;
                                    if (asym != 0)
                                    {
                                        sfac = -1F;
                                    }
                                }
                            }
                            newdata[himg] += olddata[hpx] * hi[i] * sfac;
                            hpx += hpxstr;
                        }
                        himg += stride;
                    }
                    if (hspx == hp0)
                    {
                        if (hle2 != 0)
                        {
                            hspxstr = 0;
                            hle2 = 0;
                        }
                        else
                        {
                            hspxstr = pstr;
                            osfac = 1F;
                        }
                    }
                    hspx += hspxstr;
                    hstap = 1;
                }


                if (da_ev != 0)
                {
                    if (lotap != 0)
                    {
                        lstap = 1;
                    }
                    else
                    {
                        lstap = 0;
                    }
                }
                else if (lotap != 0)
                {
                    lstap = 2;
                }
                else
                {
                    lstap = 1;
                }

                for (tap = 1; tap >= lstap; tap--)
                {
                    lle = lle2;
                    lre = lre2;
                    lpx = lspx;
                    lpxstr = lspxstr;

                    newdata[limg] = olddata[lpx] * lo[tap];
                    for (i = tap + 2; i < lsz; i += 2)
                    {
                        if (lpx == lp0)
                        {
                            if (lle != 0)
                            {
                                lpxstr = 0;
                                lle = 0;
                            }
                            else
                            {
                                lpxstr = pstr;
                            }
                        }
                        if (lpx == lp1)
                        {
                            if (lre != 0)
                            {
                                lpxstr = 0;
                                lre = 0;
                            }
                            else
                            {
                                lpxstr = nstr;
                            }
                        }
                        lpx += lpxstr;
                        newdata[limg] += olddata[lpx] * lo[i];
                    }
                    limg += stride;
                }

                if (da_ev != 0)
                {
                    if (hotap != 0)
                    {
                        hstap = 1;
                    }
                    else
                    {
                        hstap = 0;
                    }

                    if (hsz == 2)
                    {
                        hspx -= hspxstr;
                        fhre = 1;
                    }
                }
                else if (hotap != 0)
                {
                    hstap = 2;
                }
                else
                {
                    hstap = 1;
                }

                for (tap = 1; tap >= hstap; tap--)
                {
                    hle = hle2;
                    hre = hre2;
                    hpx = hspx;
                    hpxstr = hspxstr;
                    sfac = osfac;
                    if (hsz != 2)
                    {
                        fhre = ofhre;
                    }

                    for (i = tap; i < hsz; i += 2)
                    {
                        if (hpx == hp0)
                        {
                            if (hle != 0)
                            {
                                hpxstr = 0;
                                hle = 0;
                            }
                            else
                            {
                                hpxstr = pstr;
                                sfac = 1.0f;
                            }
                        }
                        if (hpx == hp1)
                        {
                            if (hre != 0)
                            {
                                hpxstr = 0;
                                hre = 0;
                                if (asym != 0 && da_ev != 0)
                                {
                                    hre = 1;
                                    fhre--;
                                    sfac = fhre;
                                    if (sfac == 0F)
                                    {
                                        hre = 0;
                                    }
                                }
                            }
                            else
                            {
                                hpxstr = nstr;
                                if (asym != 0)
                                {
                                    sfac = -1F;
                                }
                            }
                        }
                        newdata[himg] += olddata[hpx] * hi[i] * sfac;
                        hpx += hpxstr;
                    }
                    himg += stride;
                }
            }

            if (fi_ev == 0)
            {
                for (i = 0; i < hsz; i++)
                {
                    hi[i] = -hi[i];
                }
            }
        }

        private static void Deconstruct(WsqPixelBuffer<float> image, Quantizer quantizer,
            Filter filter)
        {
            if (quantizer.WTree != null)
            {
                float[] temp = new float[image.Length];
                WTree wtree = quantizer.WTree;
                for (int node = 0; node < WTree.Length; node++)
                {
                    int idx = (wtree[node].Y * image.Width) + wtree[node].X;
                    GetLets(temp, image.Pixels, 0, idx,
                        wtree[node].LenY, wtree[node].LenX,
                        image.Width, 1,
                        filter.Hi, filter.Lo, wtree[node].InvR);
                    GetLets(image.Pixels, temp, idx, 0,
                        wtree[node].LenX, wtree[node].LenY,
                        1, image.Width,
                        filter.Hi, filter.Lo, wtree[node].InvC);
                }
            }
            else
            {
                throw new WsqCodecException(
                    "Quantizer WTree is null or invalid");
            }
        }

        private static void GetLets(
            float[] newdata,
            float[] olddata,
            int newIndex,
            int oldIndex,
            int len1,
            int len2,
            int pitch,
            int stride,
            float[] hiflt,
            float[] loflt,
            int inv)
        {
            int lopass_idx, hipass_idx;
            int p0_idx, p1_idx;
            int da_ev;
            int fi_ev;
            int loc, hoc, nstr, pstr;
            int llen, hlen;
            int lpxstr, lspxstr;
            int lpxIndex, lspxIndex;
            int hpxstr, hspxstr;
            int hpxIndex, hspxIndex;
            int olle, ohle;
            int olre, ohre;
            int lle, lle2;
            int lre, lre2;
            int hle, hle2;
            int hre, hre2;

            da_ev = len2 % 2;
            fi_ev = loflt.Length % 2;

            if (fi_ev != 0)
            {
                loc = (loflt.Length - 1) / 2;
                hoc = ((hiflt.Length - 1) / 2) - 1;
                olle = 0;
                ohle = 0;
                olre = 0;
                ohre = 0;
            }
            else
            {
                loc = (loflt.Length / 2) - 2;
                hoc = (hiflt.Length / 2) - 2;
                olle = 1;
                ohle = 1;
                olre = 1;
                ohre = 1;

                if (loc == -1)
                {
                    loc = 0;
                    olle = 0;
                }
                if (hoc == -1)
                {
                    hoc = 0;
                    ohle = 0;
                }

                for (int i = 0; i < hiflt.Length; i++)
                {
                    hiflt[i] = -hiflt[i];
                }
            }

            pstr = stride;
            nstr = -pstr;

            if (da_ev != 0)
            {
                llen = (len2 + 1) / 2;
                hlen = llen - 1;
            }
            else
            {
                llen = len2 / 2;
                hlen = llen;
            }

            for (int rw_cl = 0; rw_cl < len1; rw_cl++)
            {
                if (inv != 0)
                {
                    hipass_idx = newIndex + (rw_cl * pitch);
                    lopass_idx = hipass_idx + (hlen * stride);
                }
                else
                {
                    lopass_idx = newIndex + (rw_cl * pitch);
                    hipass_idx = lopass_idx + (llen * stride);
                }

                p0_idx = oldIndex + (rw_cl * pitch);
                p1_idx = p0_idx + ((len2 - 1) * stride);

                lspxIndex = p0_idx + (loc * stride);
                lspxstr = nstr;
                lle2 = olle;
                lre2 = olre;
                hspxIndex = p0_idx + (hoc * stride);
                hspxstr = nstr;
                hle2 = ohle;
                hre2 = ohre;
                for (int pix = 0; pix < hlen; pix++)
                {
                    lpxstr = lspxstr;
                    lpxIndex = lspxIndex;
                    lle = lle2;
                    lre = lre2;
                    newdata[lopass_idx] = olddata[lpxIndex] * loflt[0];
                    for (int i = 1; i < loflt.Length; i++)
                    {
                        if (lpxIndex == p0_idx)
                        {
                            if (lle != 0)
                            {
                                lpxstr = 0;
                                lle = 0;
                            }
                            else
                            {
                                lpxstr = pstr;
                            }
                        }
                        if (lpxIndex == p1_idx)
                        {
                            if (lre != 0)
                            {
                                lpxstr = 0;
                                lre = 0;
                            }
                            else
                            {
                                lpxstr = nstr;
                            }
                        }
                        lpxIndex += lpxstr;
                        newdata[lopass_idx] += olddata[lpxIndex] * loflt[i];
                    }
                    lopass_idx += stride;

                    hpxstr = hspxstr;
                    hpxIndex = hspxIndex;
                    hle = hle2;
                    hre = hre2;
                    newdata[hipass_idx] = olddata[hpxIndex] * hiflt[0];
                    for (int i = 1; i < hiflt.Length; i++)
                    {
                        if (hpxIndex == p0_idx)
                        {
                            if (hle != 0)
                            {
                                hpxstr = 0;
                                hle = 0;
                            }
                            else
                            {
                                hpxstr = pstr;
                            }
                        }
                        if (hpxIndex == p1_idx)
                        {
                            if (hre != 0)
                            {
                                hpxstr = 0;
                                hre = 0;
                            }
                            else
                            {
                                hpxstr = nstr;
                            }
                        }
                        hpxIndex += hpxstr;
                        newdata[hipass_idx] += olddata[hpxIndex] * hiflt[i];
                    }
                    hipass_idx += stride;

                    for (int i = 0; i < 2; i++)
                    {
                        if (lspxIndex == p0_idx)
                        {
                            if (lle2 != 0)
                            {
                                lspxstr = 0;
                                lle2 = 0;
                            }
                            else
                            {
                                lspxstr = pstr;
                            }
                        }
                        lspxIndex += lspxstr;
                        if (hspxIndex == p0_idx)
                        {
                            if (hle2 != 0)
                            {
                                hspxstr = 0;
                                hle2 = 0;
                            }
                            else
                            {
                                hspxstr = pstr;
                            }
                        }
                        hspxIndex += hspxstr;
                    }
                }
                if (da_ev != 0)
                {
                    lpxstr = lspxstr;
                    lpxIndex = lspxIndex;
                    lle = lle2;
                    lre = lre2;
                    newdata[lopass_idx] = olddata[lpxIndex] * loflt[0];
                    for (int i = 1; i < loflt.Length; i++)
                    {
                        if (lpxIndex == p0_idx)
                        {
                            if (lle != 0)
                            {
                                lpxstr = 0;
                                lle = 0;
                            }
                            else
                            {
                                lpxstr = pstr;
                            }
                        }
                        if (lpxIndex == p1_idx)
                        {
                            if (lre != 0)
                            {
                                lpxstr = 0;
                                lre = 0;
                            }
                            else
                            {
                                lpxstr = nstr;
                            }
                        }
                        lpxIndex += lpxstr;
                        newdata[lopass_idx] += olddata[lpxIndex] * loflt[i];
                    }
                    //lopass_idx += stride;
                }
            }
            if (fi_ev == 0)
            {
                for (int i = 0; i < hiflt.Length; i++)
                {
                    hiflt[i] = -hiflt[i];
                }
            }
        }

        internal static Transformer Construct(WsqPixelBuffer<float> image, Dtt dtt, Quantizer quantizer)
        {
            var transformer = new Transformer();
            Reconstruct(image, dtt, quantizer);
            return transformer;
        }

        internal static Transformer Decompose(
            WsqPixelBuffer<float> image, Quantizer quantizer, Filter filter)
        {
            var transformer = new Transformer();
            Deconstruct(image, quantizer, filter);
            return transformer;
        }
    }
}
