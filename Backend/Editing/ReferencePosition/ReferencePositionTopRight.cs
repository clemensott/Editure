using System.Windows;

namespace Editure.Backend.Editing.ReferencePosition
{
    class ReferencePositionTopRight : IReferencePosition
    {
        public bool IsTopLeft => false;

        public bool IsTopCenter => false;

        public bool IsTopRight => true;

        public bool IsCenterCenter => false;

        public bool IsCenterLeft => false;

        public bool IsCenterRight => false;

        public bool IsBottomLeft => false;

        public bool IsBottomCenter => false;

        public bool IsBottomRight => false;

        public Int32Rect GetCrop(IntSize scale, IntSize show, IntPoint relativeOffset)
        {
            int x = scale.Width - show.Width - relativeOffset.X;
            int y = relativeOffset.Y;
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
            y = crop.Y;

            return new IntPoint(x, y);
        }
    }
}
