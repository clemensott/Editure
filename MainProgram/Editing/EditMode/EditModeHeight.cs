using System.Drawing;

namespace MainProgram
{
    class EditModeHeight : IEditMode
    {
        public bool IsCrop { get { return false; } }

        public bool IsRatio { get { return false; } }

        public bool IsWidth { get { return false; } }

        public bool IsHeight { get { return true; } }

        public IntSize GetScaleSize(IntSize wanna, IntSize original)
        {
            if (original.Height == 0) return new IntSize(0, wanna.Height);

            return new IntSize(original.Width * wanna.Height / original.Height, wanna.Height);
        }

        public IntSize GetShowSize(IntSize wanna, IntSize scale)
        {
            return scale;
        }
    }
}
