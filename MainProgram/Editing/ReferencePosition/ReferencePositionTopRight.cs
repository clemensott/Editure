using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainProgram
{
    class ReferencePositionTopRight : IReferencePosition
    {
        public bool IsTopLeft { get { return false; } }

        public bool IsTopCenter { get { return false; } }

        public bool IsTopRight { get { return true; } }

        public bool IsCenterCenter { get { return false; } }

        public bool IsCenterLeft { get { return false; } }

        public bool IsCenterRight { get { return false; } }

        public bool IsBottomLeft { get { return false; } }

        public bool IsBottomCenter { get { return false; } }

        public bool IsBottomRight { get { return false; } }

        public Int32Rect GetCrop(IntSize scale, IntSize show, IntPoint relativeOffset)
        {
            int x, y, width, height;

            x = scale.Width - show.Width - relativeOffset.X;
            y = relativeOffset.Y;
            width = show.Width;
            height = show.Height;

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
