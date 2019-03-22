using FolderFile;

namespace MainProgram.Save
{
    public class SearchInfo
    {
        public bool WithExtentions;
        public SerializableFolder? Src, Ref;
        public DestinationFolder SrcFound, SrcNot, RefFound;

        public SearchInfo()
        {
            WithExtentions = false;

            Src = null;
            Ref = null;

            SrcFound = new DestinationFolder();
            SrcNot = new DestinationFolder();
            RefFound = new DestinationFolder();
        }
    }
}
