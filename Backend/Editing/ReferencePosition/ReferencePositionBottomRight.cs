using System.Windows;

namespace Editure.Backend.Editing.ReferencePosition
{
    class ReferencePositionBottomRight : IReferencePosition
    {
        public bool IsTopLeft => false;

        public bool IsTopCenter => false;

        public bool IsTopRight => false;

        public bool IsCenterCenter => false;

        public bool IsCenterLeft => false;

        public bool IsCenterRight => false;

        public bool IsBottomLeft => false;

        public bool IsBottomCenter => false;

        public bool IsBottomRight => true;

        public Int32Rect GetCrop(IntSize scale, IntSize show, IntPoint relativeOffset)
        {
            int x = scale.Width - show.Width - relativeOffset.X;
            int y = scale.Height - show.Height - relativeOffset.Y;
            int width = show.Width;
            int height = show.Height;

            if (x + width > scale.Width) x = scale.Width - width;
            if (y + height > scale.Height) y = scale.Height - height;

            return new Int32Rect(x, y, width, height);
        }

        public IntPoint GetOffsetPoint(Int32Rect crop, IntSize scale, IntSize show)
        {
            int x, y;

            x = scale.Width - (crop.X + crop.Width);
            y = scale.Height - (crop.Y + crop.Height);

            return new IntPoint(x, y);
        }
    }
}
