// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/license.txt

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace BiomSharp.Primitives
{
    /// <summary>
    /// Represents an ordered pair of x and y coordinates that define a point in a two-dimensional plane.
    /// </summary>
    [Serializable]
    public struct PointF : IEquatable<PointF>
    {
        /// <summary>
        /// Creates a new instance of the <see cref='PointF'/> class with member data left uninitialized.
        /// </summary>
        public static readonly PointF Empty;
        private float x; // Do not rename (binary serialization)
        private float y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='PointF'/> class with the specified coordinates.
        /// </summary>
        public PointF(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='PointF'/> struct from the specified
        /// <see cref="Vector2"/>.
        /// </summary>
        public PointF(Vector2 vector)
        {
            x = vector.X;
            y = vector.Y;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> from this <see cref="System.Drawing.PointF"/>.
        /// </summary>
        public Vector2 ToVector2() => new(x, y);

        /// <summary>
        /// Gets a value indicating whether this <see cref='PointF'/> is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => x == 0f && y == 0f;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='PointF'/>.
        /// </summary>
        public float X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='PointF'/>.
        /// </summary>
        public float Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Converts the specified <see cref="System.Drawing.PointF"/> to a <see cref="Vector2"/>.
        /// </summary>
        public static explicit operator Vector2(PointF point) => point.ToVector2();

        /// <summary>
        /// Converts the specified <see cref="Vector2"/> to a <see cref="System.Drawing.PointF"/>.
        /// </summary>
        public static explicit operator PointF(Vector2 vector) => new(vector);

        /// <summary>
        /// Translates a <see cref='PointF'/> by a given <see cref='Size'/> .
        /// </summary>
        public static PointF operator +(PointF pt, Size sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointF'/> by the negative of a given <see cref='Size'/> .
        /// </summary>
        public static PointF operator -(PointF pt, Size sz) => Subtract(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointF'/> by a given <see cref='SizeF'/> .
        /// </summary>
        public static PointF operator +(PointF pt, SizeF sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='PointF'/> by the negative of a given <see cref='SizeF'/> .
        /// </summary>
        public static PointF operator -(PointF pt, SizeF sz) => Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='PointF'/> objects. The result specifies whether the values of the
        /// <see cref='X'/> and <see cref='Y'/> properties of the two
        /// <see cref='PointF'/> objects are equal.
        /// </summary>
        public static bool operator ==(PointF left, PointF right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='PointF'/> objects. The result specifies whether the values of the
        /// <see cref='X'/> or <see cref='Y'/> properties of the two
        /// <see cref='PointF'/> objects are unequal.
        /// </summary>
        public static bool operator !=(PointF left, PointF right) => !(left == right);

        /// <summary>
        /// Translates a <see cref='PointF'/> by a given <see cref='Size'/> .
        /// </summary>
        public static PointF Add(PointF pt, Size sz) => new(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='PointF'/> by the negative of a given <see cref='Size'/> .
        /// </summary>
        public static PointF Subtract(PointF pt, Size sz) => new(pt.X - sz.Width, pt.Y - sz.Height);

        /// <summary>
        /// Translates a <see cref='PointF'/> by a given <see cref='SizeF'/> .
        /// </summary>
        public static PointF Add(PointF pt, SizeF sz) => new(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='PointF'/> by the negative of a given <see cref='SizeF'/> .
        /// </summary>
        public static PointF Subtract(PointF pt, SizeF sz) => new(pt.X - sz.Width, pt.Y - sz.Height);

        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is PointF pointF && Equals(pointF);

        public readonly bool Equals(PointF other) => this == other;

        public override readonly int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());

        public override readonly string ToString() => $"{{X={x}, Y={y}}}";
    }
}

