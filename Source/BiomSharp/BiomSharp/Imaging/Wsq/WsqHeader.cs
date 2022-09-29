// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.Text;
using BiomSharp.Imaging.Wsq.Segment;
using BiomSharp.Nist;

namespace BiomSharp.Imaging.Wsq
{
    internal class WsqHeader : WsqFeatures
    {
        private readonly List<string> features =
            new(
                new[]
                {
                    NistConstants.NcmHeader,
                    NistConstants.NcmPixWidth,
                    NistConstants.NcmPixHeight,
                    NistConstants.NcmPixDepth,
                    NistConstants.NcmPpi,
                    NistConstants.NcmLossy,
                    NistConstants.NcmColorSpace,
                    NistConstants.NcmCompression,
                    NistConstants.NcmWsqRate,
                });
        public WsqHeader()
        {
            AddOrUpdate(NistConstants.NcmHeader, 0);
            AddOrUpdate(NistConstants.NcmPixWidth, 0);
            AddOrUpdate(NistConstants.NcmPixHeight, 0);
            AddOrUpdate(NistConstants.NcmPixDepth, 0);
            AddOrUpdate(NistConstants.NcmPpi, 0);
            AddOrUpdate(NistConstants.NcmLossy, 1);
            AddOrUpdate(NistConstants.NcmColorSpace, "GRAY");
            AddOrUpdate(NistConstants.NcmCompression, "WSQ");
            AddOrUpdate(NistConstants.NcmWsqRate, 0);
            AddOrUpdate(NistConstants.NcmHeader, Items.Count);
        }
        public WsqHeader(int pixWidth, int pixHeight, int bitsPerPix, int pixsPerInch, float compBitRate)
            :
            this()
        {
            AddOrUpdate(NistConstants.NcmPixWidth, pixWidth.ToString());
            AddOrUpdate(NistConstants.NcmPixHeight, pixHeight.ToString());
            AddOrUpdate(NistConstants.NcmPixDepth, bitsPerPix.ToString());
            AddOrUpdate(NistConstants.NcmPpi, pixsPerInch.ToString());
            AddOrUpdate(NistConstants.NcmWsqRate, compBitRate);
        }

        public override IEnumerable<string> Names => features;

        public override bool IsValidName(string name) => features.Contains(name);

        public static WsqHeader? Deserialize(Com nistcom)
        {
            if (nistcom != null
                &&
                nistcom.Comment != null
                &&
                nistcom.Comment.StartsWith(NistConstants.NcmHeader))
            {
                var header = new WsqHeader();
                header.Combine(nistcom.Comment);
                return header;
            }
            return null;
        }

        public Com Serialize()
        {
            var ncm = new StringBuilder();
            foreach (string name in Items.Keys)
            {
                _ = ncm.AppendFormat("{0}{1}{2}{3}", name, KeyValueSeparator, Items[name], KeyValueTerminator);
            }
            return new Com(ncm.ToString());
        }
    }
}
