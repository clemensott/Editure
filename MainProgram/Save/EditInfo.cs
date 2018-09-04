using FolderFile;
using System.Drawing;

namespace MainProgram.Save
{
    public class EditInfo
    {
        public bool FlipX, FlipY;
        public string SrcPath, DestPath;
        public SubfolderType SrcSubfolderType;
        public IntSize Wanna;
        public IntPoint Offset;
        public EditMode EditMode;
        public EditReferencePositionType EditReferencePositionType;
        public EditEncoderType EditEncoder;

        public EditInfo()
        {
            FlipX = false;
            FlipY = false;

            SrcPath = string.Empty;
            DestPath = string.Empty;

            SrcSubfolderType = SubfolderType.This;

            Wanna = new IntSize();
            Offset = new IntPoint();

            EditMode = EditMode.Crop;
            EditReferencePositionType = EditReferencePositionType.CenterCenter;
            EditEncoder = EditEncoderType.Auto;
        }
    }
}
