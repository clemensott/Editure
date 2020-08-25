using System;

namespace Editure.Backend
{
    public struct IntSize : IEquatable<IntSize>
    {
        public static readonly IntSize Empty = new IntSize();

        public int Width { get; set; }

        public int Height { get; set; }

        public IntSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override bool Equals(object obj)
        {
            return obj is IntSize && Equals((IntSize)obj);
        }

        public bool Equals(IntSize other)
        {
            return Width == other.Width &&
                   Height == other.Height;
        }

        public override int GetHashCode()
        {
            var hashCode = 859600377;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(IntSize size1, IntSize size2)
        {
            return size1.Equals(size2);
        }

        public static bool operator !=(IntSize size1, IntSize size2)
        {
            return !(size1 == size2);
        }
    }
}
