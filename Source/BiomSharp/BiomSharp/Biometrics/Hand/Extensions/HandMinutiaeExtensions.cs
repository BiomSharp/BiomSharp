// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Biometrics.Hand.Extensions
{
    internal static class HandMinutiaeExtensions
    {
        // Nist = NBIS MINDTCT coord
        public static void NistToIsoCoord(this HandMinutiae minutiae)
        {
            //minutiae
            //    .Features
            //    .ToList()
            //    .ForEach(m => m.NistToIsoCoord(minutiae.SourceBounds));
        }

        // Iso = ISO-IEC Minutiae Data Format 19794-2
        public static void IsoToNistCoord(this HandMinutiae minutiae)
        {
            //minutiae
            //    .Features
            //    .ToList()
            //    .ForEach(m => m.IsoToNistCoord(minutiae.SourceBounds));
        }
    }
}
