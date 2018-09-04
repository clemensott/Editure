using System.Drawing;

namespace MainProgram
{
    class EditModeRatio : IEditMode
    {
        public bool IsCrop { get { return false; } }

        public bool IsRatio { get { return true; } }

        public bool IsWidth { get { return false; } }

        public bool IsHeight { get { return false; } }

        public IntSize GetScaleSize(IntSize wanna, IntSize original)
        {
            return wanna;
        }

        public IntSize GetShowSize(IntSize wanna, IntSize scale)
        {
            return wanna;
        }
    }
}
