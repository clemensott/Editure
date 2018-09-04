namespace MainProgram
{
    public struct IntSize
    {
        public static readonly IntSize Empty = new IntSize();

        public int Width { get; set; }

        public int Height { get; set; }

        public IntSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static bool operator ==(IntSize s1, IntSize s2)
        {
            return s1.Width == s2.Width && s1.Height == s2.Height;
        }

        public static bool operator !=(IntSize s1, IntSize s2)
        {
            return s1.Width != s2.Width || s1.Height != s2.Height;
        }
    }
}
