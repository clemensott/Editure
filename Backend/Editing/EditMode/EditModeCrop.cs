namespace Editure.Backend.Editing.EditMode
{
    class EditModeCrop : IEditMode
    {
        public bool IsCrop => true;

        public bool IsRatio => false;

        public bool IsWidth => false;

        public bool IsHeight => false;

        public IntSize GetScaleSize(IntSize wanna, IntSize original)
        {
            double wannaRatio = wanna.Width / (double)wanna.Height;
            double originalRatio = original.Width / (double)original.Height;

            IEditMode mode = wannaRatio > originalRatio ? (IEditMode)new EditModeWidth() : new EditModeHeight();

            return mode.GetScaleSize(wanna, original);
        }

        public IntSize GetShowSize(IntSize wanna, IntSize scale)
        {
            return wanna;
        }
    }
}
