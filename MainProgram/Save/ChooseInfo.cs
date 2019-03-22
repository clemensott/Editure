using FolderFile;

namespace MainProgram.Save
{
    public class ChooseInfo
    {
        public int MinWidth, MinHeight, MaxWidth, MaxHeight;
        public SerializableFolder? Src;
        public DestinationFolder Have, Havent;

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
