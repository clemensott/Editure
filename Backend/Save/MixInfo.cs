using FolderFile;

namespace Editure.Backend.Save
{
    public class MixInfo
    {
        public bool Mix { get; set; }
        
        public bool Auto { get; set; }
        
        public SerializableFolder? Folder { get; set; }

        public MixInfo()
        {
            Mix = true;
            Auto = true;

            Folder = null;
        }
    }
}
