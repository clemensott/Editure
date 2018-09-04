using System.Drawing;

namespace MainProgram
{
    class EditModeWidth : IEditMode
    {
        public bool IsCrop { get { return false; } }

        public bool IsRatio { get { return false; } }

        public bool IsWidth { get { return true; } }

        public bool IsHeight { get { return false; } }

        public IntSize GetScaleSize(IntSize wanna, IntSize original)
        {
            if (original.Width == 0) return new IntSize(wanna.Width, 0);

            return new IntSize(wanna.Width, original.Height * wanna.Width / original.Width);
        }

        public IntSize GetShowSize(IntSize wanna, IntSize scale)
        {
            return scale;
        }
    }
}
