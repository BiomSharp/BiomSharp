// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Nist;
using BiomSharp.Primitives;

namespace BiomSharp.Biometrics
{
    public interface IBiometricTemplate<TFeature>
        where TFeature : IBiometricFeature
    {
        public NistFingerCode FingerCode { get; }
        public NistFingerImprint FingerImprint { get; }
        // Pixels per inch
        int Resolution { get; }
        Rectangle SourceBounds { get; }
        Rectangle FeatureBounds { get; }
        IList<TFeature> Features { get; }
        int FeatureCount { get; }
        int Quality { get; }
        void Serialize(Stream stream);
        void Deserialize(Stream stream);
    }
}
