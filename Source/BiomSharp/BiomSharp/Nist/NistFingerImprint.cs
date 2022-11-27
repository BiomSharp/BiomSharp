// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

namespace BiomSharp.Nist
{
    // Interpol version 06.00.01 - based on ANSI/NIST ITL 2011 Upd 2015
    // Table A.31.: Table of Fingerprint Impression Types
    // Reference: https://github.com/INTERPOL-Innovation-Centre/ANSI-NIST-XML-ITL-Implementation
    public enum NistFingerImprint
    {
        // Finger(s) / palm / plantar presented on platen or paper without rolling
        Plain = 0,
        // Finger rolled on platen or paper
        Rolled = 1,
    };
}
