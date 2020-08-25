namespace Editure.Backend.Editing.EditMode
{
    class EditModeRatio : IEditMode
    {
        public bool IsCrop => false;

        public bool IsRatio => true;

        public bool IsWidth => false;

        public bool IsHeight => false;

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
