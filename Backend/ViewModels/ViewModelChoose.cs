using Editure.Backend.Doer;
using FolderFile;

namespace Editure.Backend.ViewModels
{
    public class ViewModelChoose : ViewModelPauseable, ITitle
    {
        private const string title = "Resolution";

        private bool isAnd = true;
        private Folder src;
        private DestinationFolder have, havent;
        private IntSize min, max;

        public bool IsAnd
        {
            get => isAnd;
            set
            {
                if (isAnd == value) return;

                isAnd = value;
                OnPropertyChanged("And");
            }
        }

        public IntSize Min
        {
            get => min;
            set
            {
                if (value == min) return;

                min = value;
                OnPropertyChanged("Min");
            }
        }

        public IntSize Max
        {
            get => max;
            set
            {
                if (value == max) return;

                max = value;
                OnPropertyChanged("Max");
            }
        }


        public Folder Src
        {
            get => src;
            set
            {
                if (src == value) return;

                src = value;
                OnPropertyChanged("Src");
            }
        }

        public DestinationFolder Have
        {
            get => have;
            set
            {
                if (have == value) return;

                have = value;
                OnPropertyChanged("Have");
            }
        }

        public DestinationFolder Havent
        {
            get => havent;
            set
            {
                if (havent == value) return;

                havent = value;
                OnPropertyChanged("Havent");
            }
        }

        public string IsChoosingButtonText => IsDoing ? "Stop" : "Search";

        public Chooser Chooser { get; }

        public string Title => title;

        public string CompleteTitle => title;

        public ViewModelChoose()
        {
            min = new IntSize(0, 0);
            max = new IntSize(1000, 1000);

            Have = new DestinationFolder()
            {
                IsDo = false,
                IsAllDelete = false,
                IsCopy = true
            };

            Havent = new DestinationFolder()
            {
                IsDo = false,
                IsAllDelete = false,
                IsCopy = true
            };

            Chooser = new Chooser(this);
        }
    }
}
