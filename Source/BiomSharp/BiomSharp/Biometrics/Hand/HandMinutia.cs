// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BiomSharp.Biometrics.Hand
{
    [Serializable]
    public sealed class HandMinutia : IHandMinutia, ICloneable
    {
        #region IHandMinutia implementation

        [Required]
        public int X { get; set; }

        [Required]

        public int Y { get; set; }

        [Required]
        public int Theta { get; set; }

        [Required]
        public HandMinutiaType Type { get; set; }

        public int? Quality { get; set; }

        // Nist = NBIS MINDTCT coord
        public void NistToIsoCoord(Rectangle sourceRect)
        {
            // X = X
            Y = sourceRect.Height - Y;
            Theta -= 128;
            if (Theta < 0)
            {
                Theta += 256;
            }

            Theta = (int)(((double)Theta * 360 / 256) + 0.5);
        }

        // Iso = ISO-IEC Minutiae Data Format 19794-2
        public void IsoToNistCoord(Rectangle sourceRect)
        {
            // X = X
            Y = sourceRect.Height - Y;
            Theta += 180;
            if (Theta >= 360)
            {
                Theta -= 360;
            }

            Theta = (int)(((double)Theta * 256 / 360) + 0.5);
        }

        #endregion IHandMinutia implementation

        #region ICloneable implementation

        public object Clone() => new HandMinutia()
        {
            X = X,
            Y = Y,
            Theta = Theta,
            Type = Type,
            Quality = Quality
        };

        #endregion ICloneable implementation
    }
}
