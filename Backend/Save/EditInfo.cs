using Editure.Backend.Editing.EditEncoders;
using Editure.Backend.Editing.EditMode;
using Editure.Backend.Editing.ReferencePosition;
using FolderFile;

namespace Editure.Backend.Save
{
    public class EditInfo
    {
        public bool FlipX { get; set; }

        public bool FlipY { get; set; }
        
        public SerializableFolder? Src { get; set; }
        
        public string DestPath { get; set; }
        
        public IntSize Wanna { get; set; }
        
        public IntPoint Offset { get; set; }
        
        public EditModeType Mode { get; set; }
        public EditReferencePositionType ReferencePositionType { get; set; }
        
        public EditEncoderType EditEncoder { get; set; }

        public EditInfo()
        {
            FlipX = false;
            FlipY = false;

            Src = null;
            DestPath = string.Empty;

            Wanna = new IntSize();
            Offset = new IntPoint();

            Mode = EditModeType.Crop;
            ReferencePositionType = EditReferencePositionType.CenterCenter;
            EditEncoder = EditEncoderType.Auto;
        }
    }
}
