using Editure.Backend.Doer;
using FolderFile;

namespace Editure.Backend.ViewModels
{
    public class ViewModelSearch : ViewModelPauseable, ITitle
    {
        private const string title = "Search";

        private bool isWithExtension;
        private Folder src, reference;
        private DestinationFolder srcFound, srcNot, refFound;

        public bool IsWithExtension
        {
            get => isWithExtension;
            set
            {
                isWithExtension = value;
                OnPropertyChanged("IsWithExtension");
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

        public Folder Ref
        {
            get => reference;
            set
            {
                if (reference == value) return;
                {
                    reference = value;
                    OnPropertyChanged("Reference");
                }
            }
        }

        public DestinationFolder SrcFound
        {
            get => srcFound;
            set
            {
                if (srcFound == value) return;

                srcFound = value;
                OnPropertyChanged("SrcFound");
            }
        }

        public DestinationFolder SrcNot
        {
            get => srcNot;
            set
            {
                if (srcNot == value) return;

                srcNot = value;
                OnPropertyChanged("SrcNot");

            }
        }

        public DestinationFolder RefFound
        {
            get => refFound;
            set
            {
                if (refFound == value) return;

                refFound = value;
                OnPropertyChanged("ReferenceFound");
            }
        }

        public Searcher Searcher { get; }

        public string Title => title;

        public string CompleteTitle => title;

        public ViewModelSearch()
        {
            Src = null;
            Ref = null;

            RefFound = new DestinationFolder()
            {
                IsDo = false,
                IsAllDelete = false,
                IsCopy = true,
                Dest = null
            };

            SrcFound = new DestinationFolder()
            {
                IsDo = false,
                IsAllDelete = false,
                IsCopy = true,
                Dest = null
            };

            SrcNot = new DestinationFolder()
            {
                IsDo = false,
                IsAllDelete = false,
                IsCopy = true,
                Dest = null
            };

            Searcher = new Searcher(this);
        }
    }
}
