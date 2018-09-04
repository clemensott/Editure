using System;
using System.Windows;

namespace MainProgram
{
    public struct IntPoint
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

        public static bool operator ==(IntPoint p1, IntPoint p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(IntPoint p1, IntPoint p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public static implicit operator IntPoint(Point point)
        {
            return new IntPoint((int)point.X, (int)point.Y);
        }

        public static implicit operator Point(IntPoint point)
        {
            return new Point(point.X, point.Y);
        }
    }
}
