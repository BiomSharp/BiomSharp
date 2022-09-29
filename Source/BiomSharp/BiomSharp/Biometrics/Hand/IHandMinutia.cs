// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Biometrics.Hand
{
    /// <summary>
    /// Standard hand/finger-print minutia definition
    /// </summary>
    /// <remarks>
    /// Coordinates as per ISO-IEC Minutiae Data Format 19794-2.
    /// </remarks>
    public interface IHandMinutia : IBiometricFeature
    {
        /// <summary>
        /// Minutia horizontal offset from image top left origin (0,0).
        /// </summary>
        int X { get; }
        /// <summary>
        /// Minutia vertical offset from image top left origin (0,0).
        /// </summary>        
        int Y { get; }
        /// <summary>
        /// The mean direction of the tangents to the two valleys 
        /// enclosing the minutia termination and is measured increasing 
        /// counter-clockwise, expressed as radians.
        /// </summary>
        int Theta { get; }
        /// <summary>
        /// The type of minutia: <see cref="HandMinutiaType"/>.
        /// </summary>
        HandMinutiaType Type { get; }
        /// <summary>
        /// A quality or confidence indicator or measure, usually expressed in
        /// the range 0..100.
        /// </summary>
        int? Quality { get; }
    }
}
