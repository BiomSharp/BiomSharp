// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging;

namespace BiomSharp.Biometrics.Hand
{
    public sealed class HandMinutiaExtractor : IBiometricExtractor<HandMinutia>
    {
        public IBiometricTemplate<HandMinutia> ExtractFeatures(IBiometricPrint biometricPrint)
        {
            SimpleBitmap? rawImage = (biometricPrint as IHandPrint)?.ToRaw();
            if (rawImage != null)
            {
                throw new NotImplementedException();
            }
            throw new InvalidOperationException("Cannot create raw image");
        }
    }
}
