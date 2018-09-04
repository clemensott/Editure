using FolderFile;

namespace MainProgram
{
    public class ViewModelChoose : ViewModelPauseable, ITitle
    {
        private const string title = "Auflösung";

        private bool isAnd = true;
        private Folder src;
        private DestinationFolder have, havent;
        private IntSize min, max;

        public bool IsAnd
        {
            get { return isAnd; }
            set
            {
                if (isAnd == value) return;

                isAnd = value;
                OnPropertyChanged("And");
            }
        }

        public IntSize Min
        {
            get { return min; }
            set
            {
                if (value == min) return;

                min = value;
                OnPropertyChanged("Min");
            }
        }

        public IntSize Max
        {
            get { return max; }
            set
            {
                if (value == max) return;

                max = value;
                OnPropertyChanged("Max");
            }
        }


        public Folder Src
        {
            get { return src; }
            set
            {
                if (src == value) return;

                src = value;
                OnPropertyChanged("Src");
            }
        }

        public DestinationFolder Have
        {
            get { return have; }
            set
            {
                if (have == value) return;

                have = value;
                OnPropertyChanged("Have");
            }
        }

        public DestinationFolder Havent
        {
            get { return havent; }
            set
            {
                if (havent == value) return;

                havent = value;
                OnPropertyChanged("Havent");
            }
        }

        public string IsChoosingButtonText { get { return IsDoing ? "Stop" : "Auswählen"; } }

        public Chooser Chooser { get; private set; }

        public string Title => title;

        public string CompleteTitle => title;

        public ViewModelChoose()
        {
            min = new IntSize(0, 0);
            max = new IntSize(1000, 1000);

            Src = new Folder("", SubfolderType.This);

            Have = new DestinationFolder()
            {
                IsDo = false,
                IsAllDelete = false,
                IsCopy = true,
                Dest = new Folder("", SubfolderType.This)
            };

            Havent = new DestinationFolder()
            {
                IsDo = false,
                IsAllDelete = false,
                IsCopy = true,
                Dest = new Folder("", SubfolderType.This)
            };

            Chooser = new Chooser(this);
        }
    }
}
