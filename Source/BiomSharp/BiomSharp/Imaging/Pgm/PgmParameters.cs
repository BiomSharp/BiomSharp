// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Imaging.Pgm
{
    public class PgmParameters
    {
        public PgmFormatType Format { get; set; } = PgmFormatType.BIN;

        public string[] Comments { get; set; } = Array.Empty<string>();
    }
}
