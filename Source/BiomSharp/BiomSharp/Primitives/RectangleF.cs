// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace BiomSharp.Primitives
{
    /// <summary>
    /// Stores the location and size of a rectangular region.
    /// </summary>
    [Serializable]
    public struct RectangleF : IEquatable<RectangleF>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='RectangleF'/> class.
        /// </summary>
        public static readonly RectangleF Empty;

        private float x; // Do not rename (binary serialization)
        private float y; // Do not rename (binary serialization)
        private float width; // Do not rename (binary serialization)
        private float height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='RectangleF'/> class with the specified location
        /// and size.
        /// </summary>
        public RectangleF(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='RectangleF'/> class with the specified location
        /// and size.
        /// </summary>
        public RectangleF(PointF location, SizeF size)
        {
            x = location.X;
            y = location.Y;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='RectangleF'/> struct from the specified
        /// <see cref="Vector4"/>.
        /// </summary>
        public RectangleF(Vector4 vector)
        {
            x = vector.X;
            y = vector.Y;
            width = vector.Z;
            height = vector.W;
        }

        /// <summary>
        /// Creates a new <see cref="Vector4"/> from this <see cref="System.Drawing.RectangleF"/>.
        /// </summary>
        public Vector4 ToVector4() => new(x, y, width, height);

        /// <summary>
        /// Converts the specified <see cref="System.Drawing.RectangleF"/> to a <see cref="Vector4"/>.
        /// </summary>
        public static explicit operator Vector4(RectangleF rectangle) => rectangle.ToVector4();

        /// <summary>
        /// Converts the specified <see cref="Vector2"/> to a <see cref="System.Drawing.RectangleF"/>.
        /// </summary>
        public static explicit operator RectangleF(Vector4 vector) => new(vector);

        /// <summary>
        /// Creates a new <see cref='RectangleF'/> with the specified location and size.
        /// </summary>
        public static RectangleF FromLTRB(float left, float top, float right, float bottom) =>
            new(left, top, right - left, bottom - top);

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular region represented by this
        /// <see cref='RectangleF'/>.
        /// </summary>
        [Browsable(false)]
        public PointF Location
        {
            readonly get => new(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref='RectangleF'/>.
        /// </summary>
        [Browsable(false)]
        public SizeF Size
        {
            readonly get => new(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='RectangleF'/>.
        /// </summary>
        public float X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='RectangleF'/>.
        /// </summary>
        public float Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref='RectangleF'/>.
        /// </summary>
        public float Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Gets or sets the height of the rectangular region defined by this <see cref='RectangleF'/>.
        /// </summary>
        public float Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='RectangleF'/> .
        /// </summary>
        [Browsable(false)]
        public readonly float Left => X;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='RectangleF'/>.
        /// </summary>
        [Browsable(false)]
        public readonly float Top => Y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='RectangleF'/>.
        /// </summary>
        [Browsable(false)]
        public readonly float Right => X + Width;

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='RectangleF'/>.
        /// </summary>
        [Browsable(false)]
        public readonly float Bottom => Y + Height;

        /// <summary>
        /// Tests whether this <see cref='RectangleF'/> has a <see cref='Width'/> or a <see cref='Height'/> of 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => Width <= 0 || Height <= 0;

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref='RectangleF'/> with the same location and
        /// size of this <see cref='RectangleF'/>.
        /// </summary>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is RectangleF rectangleF && Equals(rectangleF);

        public readonly bool Equals(RectangleF other) => this == other;

        /// <summary>
        /// Tests whether two <see cref='RectangleF'/> objects have equal location and size.
        /// </summary>
        public static bool operator ==(RectangleF left, RectangleF right) =>
            left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;

        /// <summary>
        /// Tests whether two <see cref='RectangleF'/> objects differ in location or size.
        /// </summary>
        public static bool operator !=(RectangleF left, RectangleF right) => !(left == right);

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='Rectangle'/> .
        /// </summary>
        public readonly bool Contains(float x, float y) => X <= x && x < X + Width && Y <= y && y < Y + Height;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='Rectangle'/> .
        /// </summary>
        public readonly bool Contains(PointF pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is entirely contained within
        /// the rectangular region represented by this <see cref='Rectangle'/> .
        /// </summary>
        public readonly bool Contains(RectangleF rect) =>
            X <= rect.X && rect.X + rect.Width <= X + Width && Y <= rect.Y && rect.Y + rect.Height <= Y + Height;

        /// <summary>
        /// Gets the hash code for this <see cref='RectangleF'/>.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        /// <summary>
        /// Inflates this <see cref='Rectangle'/> by the specified amount.
        /// </summary>
        public void Inflate(float x, float y)
        {
            X -= x;
            Y -= y;
            Width += 2 * x;
            Height += 2 * y;
        }

        /// <summary>
        /// Inflates this <see cref='Rectangle'/> by the specified amount.
        /// </summary>
        public void Inflate(SizeF size) => Inflate(size.Width, size.Height);

        /// <summary>
        /// Creates a <see cref='Rectangle'/> that is inflated by the specified amount.
        /// </summary>
        public static RectangleF Inflate(RectangleF rect, float x, float y)
        {
            RectangleF r = rect;
            r.Inflate(x, y);
            return r;
        }

        /// <summary>
        /// Creates a Rectangle that represents the intersection between this Rectangle and rect.
        /// </summary>
        public void Intersect(RectangleF rect)
        {
            RectangleF result = Intersect(rect, this);

            X = result.X;
            Y = result.Y;
            Width = result.Width;
            Height = result.Height;
        }

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and b. If there is no intersection, an
        /// empty rectangle is returned.
        /// </summary>
        public static RectangleF Intersect(RectangleF a, RectangleF b)
        {
            float x1 = Math.Max(a.X, b.X);
            float x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            float y1 = Math.Max(a.Y, b.Y);
            float y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new RectangleF(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(RectangleF rect) =>
            rect.X < X + Width && X < rect.X + rect.Width && rect.Y < Y + Height && Y < rect.Y + rect.Height;

        /// <summary>
        /// Creates a rectangle that represents the union between a and b.
        /// </summary>
        public static RectangleF Union(RectangleF a, RectangleF b)
        {
            float x1 = Math.Min(a.X, b.X);
            float x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            float y1 = Math.Min(a.Y, b.Y);
            float y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new RectangleF(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(PointF pos) => Offset(pos.X, pos.Y);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(float x, float y)
        {
            X += x;
            Y += y;
        }

        /// <summary>
        /// Converts the specified <see cref='Rectangle'/> to a
        /// <see cref='RectangleF'/>.
        /// </summary>
        public static implicit operator RectangleF(Rectangle r) => new(r.X, r.Y, r.Width, r.Height);

        /// <summary>
        /// Converts the <see cref='Location'/> and <see cref='Size'/>
        /// of this <see cref='RectangleF'/> to a human-readable string.
        /// </summary>
        public override readonly string ToString() => $"{{X={X},Y={Y},Width={Width},Height={Height}}}";
    }
}
