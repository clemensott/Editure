using FolderFile;

namespace Editure.Backend.Save
{
    public class ChooseInfo
    {
        public int MinWidth { get; set; }
        
        public int MinHeight { get; set; }
        
        public int MaxWidth { get; set; }
        
        public int MaxHeight { get; set; }

        public SerializableFolder? Src { get; set; }
        
        public DestinationFolder Have { get; set; }
        
        public DestinationFolder Havent { get; set; }

        public ChooseInfo()
        {
            MinWidth = 0;
            MinHeight = 0;
            MaxWidth = 0;
            MaxHeight = 0;

            Src = null;

            Have = new DestinationFolder();
            Havent = new DestinationFolder();

        }
    }
}
