// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

namespace BiomSharp.Biometrics
{
    public interface IBiometricExtractor<TFeature> where TFeature : IBiometricFeature
    {
        IBiometricTemplate<TFeature> ExtractFeatures(IBiometricPrint biometricPrint);
    }
}
