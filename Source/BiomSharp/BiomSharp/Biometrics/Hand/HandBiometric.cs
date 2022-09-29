// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel.DataAnnotations;
using BiomSharp.Nist;

namespace BiomSharp.Biometrics.Hand
{
    [Serializable]
    public sealed class HandBiometric<F> where F : struct, Enum
    {
        // Capture sequence if multiple, and required.
        public int? Sequence { get; set; }

        // Imprint: rolled, plain.
        [Required]
        public NistFingerImprint Imprint { get; set; }

        // NIST finger/hand print code.
        [Required]
        public NistFingerCode Code { get; set; } = NistFingerCode.NullObject;

        // Handprint
        public HandPrint<F>? HandPrint { get; set; }

        // Template
        public HandMinutiae? HandMinutiae { get; set; }
    }
}
