using FolderFile;

namespace Editure.Backend.Save
{
    public class CopyInfo
    {
        public string DestPath { get; set; }
        
        public SerializableFolder? Src { get; set; }

        public CopyInfo()
        {
            Src = null;
            DestPath = string.Empty;
        }
    }
}
