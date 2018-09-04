using FolderFile;

namespace MainProgram.Save
{
    public class MixInfo
    {
        public bool Mix, Auto;
        public string Path;
        public SubfolderType SubfolderType;

        public MixInfo()
        {
            Mix = true;
            Auto = true;

            Path = string.Empty;

            SubfolderType = SubfolderType.This;
        }
    }
}
