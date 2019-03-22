using FolderFile;

namespace MainProgram.Save
{
    public class EditInfo
    {
        public bool FlipX, FlipY;
        public SerializableFolder? Src;
        public string DestPath;
        public IntSize Wanna;
        public IntPoint Offset;
        public EditMode EditMode;
        public EditReferencePositionType EditReferencePositionType;
        public EditEncoderType EditEncoder;

        public EditInfo()
        {
            FlipX = false;
            FlipY = false;

            Src = null;
            DestPath = string.Empty;

            Wanna = new IntSize();
            Offset = new IntPoint();

            EditMode = EditMode.Crop;
            EditReferencePositionType = EditReferencePositionType.CenterCenter;
            EditEncoder = EditEncoderType.Auto;
        }
    }
}
