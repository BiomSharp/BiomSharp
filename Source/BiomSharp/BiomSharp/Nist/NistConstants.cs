// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Nist
{
    public static class NistConstants
    {
        // NIST FET/NISTCOM constants specific to WSQ
        public const string NcmHeader = "NIST_COM";
        public const string NcmPixWidth = "PIX_WIDTH";
        public const string NcmPixHeight = "PIX_HEIGHT";
        public const string NcmPixDepth = "PIX_DEPTH"; // 1,8,24
        public const string NcmPpi = "PPI"; // -1 if unknown
        public const string NcmColorSpace = "COLORSPACE"; // RGB,YCbCr,GRAY
        public const string NcmCompression = "COMPRESSION"; // NONE,JPEGB,JPEGL,WSQ
        public const string NcmWsqRate = "WSQ_BITRATE"; // .75<=, >=2.25 (-1.0 if unknown)
        public const string NcmLossy = "LOSSY"; // 0,1

        public const int NcmPpiUnknown = -1;
    }
}
