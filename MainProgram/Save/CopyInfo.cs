using FolderFile;

namespace MainProgram.Save
{
    public class CopyInfo
    {
        public string DestPath;
        public SerializableFolder? Src;

        public CopyInfo()
        {
            Src = null;
            DestPath = string.Empty;
        }
    }
}
