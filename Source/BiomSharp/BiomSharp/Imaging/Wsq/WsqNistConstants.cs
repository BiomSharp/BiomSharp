// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Imaging.Wsq
{
    internal static class WsqNistConstants
    {
        // NIST WSQ NISTCOM constants
        public const string NCM_HEADER = "NIST_COM";
        public const string NCM_PIX_WIDTH = "PIX_WIDTH";
        public const string NCM_PIX_HEIGHT = "PIX_HEIGHT";
        public const string NCM_PIX_DEPTH = "PIX_DEPTH";
        public const string NCM_PPI = "PPI";
        public const string NCM_COLORSPACE = "COLORSPACE";
        public const string NCM_N_CMPNTS = "NUM_COMPONENTS";
        public const string NCM_COMPRESSION = "COMPRESSION";
        public const string NCM_WSQ_RATE = "WSQ_BITRATE"; // .75<=, >=2.25 (-1.0 if unknown)
        public const string NCM_LOSSY = "LOSSY";
    }
}
