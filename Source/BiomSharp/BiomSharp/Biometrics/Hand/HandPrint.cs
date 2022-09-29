// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel.DataAnnotations;
using BiomSharp.Imaging;

namespace BiomSharp.Biometrics.Hand
{
    [Serializable]
    public sealed class HandPrint<TFormat> : IHandPrint, IBiometricPrint
        where TFormat : struct, Enum
    {
        #region IBiometricPrint implementation

        // Encoded image
        [Required]
        public byte[] EncodedImage { get; private set; }

        #endregion IBiometricPrint implementation

        #region IBiometricPrint<F> implementation

        [Required]
        public TFormat Format { get; }

        // Image width in pixels
        [Required]
        public int Width { get; private set; }

        // Image height in pixels
        [Required]
        public int Height { get; private set; }

        public SimpleBitmap? ToRaw() => throw new NotImplementedException();

        #endregion IBiometricPrint<F> implementation

        public HandPrint(TFormat format, int width, int height, byte[] encodedImage)
        {
            Format = format;
            Width = width;
            Height = height;
            EncodedImage = encodedImage;
        }
    }
}
