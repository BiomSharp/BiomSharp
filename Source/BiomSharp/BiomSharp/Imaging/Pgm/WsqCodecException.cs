// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

namespace BiomSharp.Imaging.Pgm
{
    public class PgmCodecException : Exception
    {
        public PgmCodecException()
        {
        }

        public PgmCodecException(string message)
            : base(message)
        {
        }

        public PgmCodecException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
