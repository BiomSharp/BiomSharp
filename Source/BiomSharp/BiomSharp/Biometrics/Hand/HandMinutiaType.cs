// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Biometrics.Hand
{
    /// <summary>
    /// Hand/finger minutia type.
    /// </summary>
    public enum HandMinutiaType
    {
        /// <summary>
        /// Friction-ridge discontinuity which is 
        /// neither a ridge-end or bifurcation.
        /// </summary>
        Unknown,
        /// <summary>
        /// Friction-ridge termination point.
        /// </summary>
        RidgeEnd,
        /// <summary>
        /// Point where friction-ridge splits and continues 
        /// as ridges.
        /// </summary>
        Bifurcation,
    };
}
