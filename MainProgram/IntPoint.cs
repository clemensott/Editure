using System;
using System.Windows;

namespace MainProgram
{
    public struct IntPoint : IEquatable<IntPoint>
    {
        public static readonly IntPoint Empty = new IntPoint();

        public int X { get; set; }

        public int Y { get; set; }

        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Offset(IntPoint offset)
        {
            X += offset.X;
            Y += offset.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is IntPoint && Equals((IntPoint)obj);
        }

        public bool Equals(IntPoint other)
        {
            return X == other.X &&
                   Y == other.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public static implicit operator IntPoint(Point point)
        {
            return new IntPoint((int)point.X, (int)point.Y);
        }

        public static implicit operator Point(IntPoint point)
        {
            return new Point(point.X, point.Y);
        }

        public static bool operator ==(IntPoint point1, IntPoint point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(IntPoint point1, IntPoint point2)
        {
            return !(point1 == point2);
        }
    }
}
