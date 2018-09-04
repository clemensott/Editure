using FolderFile;

namespace MainProgram.Save
{
    public class CopyInfo
    {
        public string SrcPath, DestPath;
        public SubfolderType SrcSubfolderType;

        public CopyInfo()
        {
            SrcPath = string.Empty;
            DestPath = string.Empty;

            SrcSubfolderType = SubfolderType.This;
        }
    }
}
