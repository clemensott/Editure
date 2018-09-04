using FolderFile;

namespace MainProgram.Save
{
    public class SearchInfo
    {
        public bool WithExtentions;
        public string SrcPath, RefPath;
        public SubfolderType SrcSubfolderType, ReferenceSubfolderType;
        public DestinationFolder SrcFound, SrcNot, RefFound;

        public SearchInfo()
        {
            WithExtentions = false;

            SrcPath = string.Empty;
            RefPath = string.Empty;

            SrcSubfolderType = SubfolderType.This;
            ReferenceSubfolderType = SubfolderType.This;

            SrcFound = new DestinationFolder();
            SrcNot = new DestinationFolder();
            RefFound = new DestinationFolder();
        }
    }
}
