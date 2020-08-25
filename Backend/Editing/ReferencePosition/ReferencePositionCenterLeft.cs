using System;
using System.Windows;

namespace Editure.Backend.Editing.ReferencePosition
{
    class ReferencePositionCenterLeft : IReferencePosition
    {
        public bool IsTopLeft => false;

        public bool IsTopCenter => false;

        public bool IsTopRight => false;

        public bool IsCenterCenter => false;

        public bool IsCenterLeft => true;

        public bool IsCenterRight => false;

        public bool IsBottomLeft => false;

        public bool IsBottomCenter => false;

        public bool IsBottomRight => false;

        public Int32Rect GetCrop(IntSize scale, IntSize show, IntPoint relativeOffset)
        {
            int x = relativeOffset.X;
            int y = relativeOffset.Y + Convert.ToInt32((scale.Height - show.Height) / 2.0);
            int width = show.Width;
            int height = show.Height;

            if (x + width > scale.Width) x = scale.Width - width;
            if (y + height > scale.Height) y = scale.Height - height;

            return new Int32Rect(x, y, width, height);
        }

        public IntPoint GetOffsetPoint(Int32Rect crop, IntSize scale, IntSize show)
        {
            int x, y;

            x = crop.X;
            y = crop.Y - Convert.ToInt32((scale.Height - show.Height) / 2.0);

            return new IntPoint(x, y);
        }
    }
}
