using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MainProgram
{
    class ReferencePositionCenterLeft : IReferencePosition
    {
        public bool IsTopLeft { get { return false; } }

        public bool IsTopCenter { get { return false; } }

        public bool IsTopRight { get { return false; } }

        public bool IsCenterCenter { get { return false; } }

        public bool IsCenterLeft { get { return true; } }

        public bool IsCenterRight { get { return false; } }

        public bool IsBottomLeft { get { return false; } }

        public bool IsBottomCenter { get { return false; } }

        public bool IsBottomRight { get { return false; } }

        public Int32Rect GetCrop(IntSize scale, IntSize show, IntPoint relativeOffset)
        {
            int x, y, width, height;

            x = relativeOffset.X;
            y = relativeOffset.Y + Convert.ToInt32((scale.Height - show.Height) / 2.0);
            width = show.Width;
            height = show.Height;

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
