using FolderFile;

namespace MainProgram.Save
{
    public class ChooseInfo
    {
        public int MinWidth, MinHeight, MaxWidth, MaxHeight;
        public string SrcPath;
        public SubfolderType SrcSubfolderType;
        public DestinationFolder Have, Havent;

        public ChooseInfo()
        {
            MinWidth = 0;
            MinHeight = 0;
            MaxWidth = 0;
            MaxHeight = 0;

            SrcPath = string.Empty;
            SrcSubfolderType = SubfolderType.This;

            Have = new DestinationFolder();
            Havent = new DestinationFolder();

        }
    }
}
