namespace Editure.Backend.Editing.EditMode
{
    class EditModeHeight : IEditMode
    {
        public bool IsCrop => false;

        public bool IsRatio => false;

        public bool IsWidth => false;

        public bool IsHeight => true;

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
