using System;
using System.Drawing;

namespace MainProgram
{
    class EditModeCrop : IEditMode
    {
        public bool IsCrop { get { return true; } }

        public bool IsRatio { get { return false; } }

        public bool IsWidth { get { return false; } }

        public bool IsHeight { get { return false; } }

        public IntSize GetScaleSize(IntSize wanna, IntSize original)
        {
            double wannaRatio = (double)wanna.Width / (double)wanna.Height;
            double originalRatio = (double)original.Width / (double)original.Height;

            IEditMode mode = wannaRatio > originalRatio ? (IEditMode)new EditModeWidth() : new EditModeHeight();

            return mode.GetScaleSize(wanna, original);
        }

        public IntSize GetShowSize(IntSize wanna, IntSize scale)
        {
            return wanna;
        }
    }
}
