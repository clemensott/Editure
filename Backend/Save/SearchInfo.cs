using FolderFile;

namespace Editure.Backend.Save
{
    public class SearchInfo
    {
        public bool WithExtensions { get; set; }
        
        public SerializableFolder? Src { get; set; }
        
        public SerializableFolder? Ref { get; set; }
        
        public DestinationFolder SrcFound { get; set; }
        
        public DestinationFolder SrcNot { get; set; }
        
        public DestinationFolder RefFound { get; set; }

        public SearchInfo()
        {
            WithExtensions = false;

            Src = null;
            Ref = null;

            SrcFound = new DestinationFolder();
            SrcNot = new DestinationFolder();
            RefFound = new DestinationFolder();
        }
    }
}
