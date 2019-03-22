using FolderFile;

namespace MainProgram.Save
{
    public class MixInfo
    {
        public bool Mix, Auto;
        public SerializableFolder? Folder;

        public MixInfo()
        {
            Mix = true;
            Auto = true;

            Folder = null;
        }
    }
}
