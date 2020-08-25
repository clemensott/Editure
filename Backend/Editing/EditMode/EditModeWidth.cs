namespace Editure.Backend.Editing.EditMode
{
    class EditModeWidth : IEditMode
    {
        public bool IsCrop => false;

        public bool IsRatio => false;

        public bool IsWidth => true;

        public bool IsHeight => false;

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
