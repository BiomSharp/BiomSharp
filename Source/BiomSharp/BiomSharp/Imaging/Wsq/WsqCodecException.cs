// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Imaging.Wsq
{
    public class WsqCodecException : Exception
    {
        public WsqCodecException()
        {
        }

        public WsqCodecException(string message)
            : base(message)
        {
        }

        public WsqCodecException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
