using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainProgram
{
    class ReferencePositionBottomRight : IReferencePosition
    {
        public bool IsTopLeft { get { return false; } }

        public bool IsTopCenter { get { return false; } }

        public bool IsTopRight { get { return false; } }

        public bool IsCenterCenter { get { return false; } }

        public bool IsCenterLeft { get { return false; } }

        public bool IsCenterRight { get { return false; } }

        public bool IsBottomLeft { get { return false; } }

        public bool IsBottomCenter { get { return false; } }

        public bool IsBottomRight { get { return true; } }

        public Int32Rect GetCrop(IntSize scale, IntSize show, IntPoint relativeOffset)
        {
            int x, y, width, height;

            x = scale.Width - show.Width - relativeOffset.X;
            y = scale.Height - show.Height - relativeOffset.Y;
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
            y = scale.Height - (crop.Y + crop.Height);

            return new IntPoint(x, y);
        }
    }
}
