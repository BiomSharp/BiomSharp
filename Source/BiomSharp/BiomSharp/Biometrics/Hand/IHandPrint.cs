// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using BiomSharp.Imaging;

namespace BiomSharp.Biometrics.Hand
{
    public interface IHandPrint
    {
        SimpleBitmap? ToRaw();
    }
}
