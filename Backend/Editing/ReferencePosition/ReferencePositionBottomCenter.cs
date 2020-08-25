using System;
using System.Windows;

namespace Editure.Backend.Editing.ReferencePosition
{
    class ReferencePositionBottomCenter : IReferencePosition
    {
        public bool IsTopLeft => false;

        public bool IsTopCenter => false;

        public bool IsTopRight => false;

        public bool IsCenterCenter => false;

        public bool IsCenterLeft => false;

        public bool IsCenterRight => false;

        public bool IsBottomLeft => false;

        public bool IsBottomCenter => true;

        public bool IsBottomRight => false;

        public Int32Rect GetCrop(IntSize scale, IntSize show, IntPoint relativeOffset)
        {
            int x = relativeOffset.X + Convert.ToInt32((scale.Width - show.Width) / 2.0);
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

            x = crop.X - Convert.ToInt32((scale.Width - show.Width) / 2.0);
            y = scale.Height - (crop.Y + crop.Height);

            return new IntPoint(x, y);
        }
    }
}
