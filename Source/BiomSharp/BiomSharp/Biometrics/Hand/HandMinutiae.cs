// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel.DataAnnotations;
using BiomSharp.Nist;
using BiomSharp.Primitives;

namespace BiomSharp.Biometrics.Hand
{
    [Serializable]
    public sealed class HandMinutiae : IBiometricTemplate<IHandMinutia>, ICloneable
    {
        private readonly List<IHandMinutia> minutiae = new();

        #region IBiometricTemplate implementation

        [Required]
        public NistFingerCode FingerCode { get; } = NistFingerCode.NullObject;

        [Required]
        public NistFingerImprint FingerImprint { get; }

        [Required]
        public int Resolution { get; }

        [Required]
        public Rectangle SourceBounds { get; }

        [Required]
        public Rectangle FeatureBounds { get; }

        public IList<IHandMinutia> Features => minutiae;

        public int FeatureCount => minutiae.Count;

        [Required]
        public int Quality { get; set; }

        public void Deserialize(Stream stream)
        {

        }

        public void Serialize(Stream stream) => throw new NotImplementedException();

        #endregion IBiometricTemplate implementation

        #region ICloneable implementation

        public object Clone() => new HandMinutiae(this);

        #endregion ICloneable implementation

        public Rectangle CalcFeatureBounds()
        {
            Rectangle rect = Rectangle.Empty;
            minutiae.ForEach(m => rect =
                    Rectangle
                    .Union(rect, Rectangle.FromLTRB(m.X, m.Y, m.X, m.Y)));
            return rect;
        }

        public HandMinutiae(HandMinutiae minutiae)
        {
            FingerCode = minutiae.FingerCode;
            FingerImprint = minutiae.FingerImprint;
            Resolution = minutiae.Resolution;
            SourceBounds = minutiae.SourceBounds;
            Quality = minutiae.Quality;
            minutiae
                .Features
                .ToList()
                .ForEach(f => this.minutiae
                    .Add((IHandMinutia)((HandMinutia)f).Clone()));
            FeatureBounds = CalcFeatureBounds();
        }

        public HandMinutiae(
            NistFingerCode fingerCode,
            NistFingerImprint fingerImprint,
            int resolution,
            IEnumerable<HandMinutia> minutias,
            Rectangle sourceBounds,
            int quality)
        {
            FingerCode = fingerCode;
            FingerImprint = fingerImprint;
            Resolution = resolution;
            minutiae.AddRange(minutias);
            SourceBounds = sourceBounds;
            Quality = quality;
            FeatureBounds = CalcFeatureBounds();
        }
    }
}
