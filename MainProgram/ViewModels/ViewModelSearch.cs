using FolderFile;

namespace MainProgram
{
    public class ViewModelSearch : ViewModelPauseable, ITitle
    {
        private const string title = "Suchen";

        private bool isWithExtension;
        private Folder src, reference;
        private DestinationFolder srcFound, srcNot, refFound;

        public bool IsWithExtension
        {
            get { return isWithExtension; }
            set
            {
                isWithExtension = value;
                OnPropertyChanged("IsWithExtension");
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

        public Folder Ref
        {
            get { return reference; }
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
            get { return srcFound; }
            set
            {
                if (srcFound == value) return;

                srcFound = value;
                OnPropertyChanged("SrcFound");
            }
        }

        public DestinationFolder SrcNot
        {
            get { return srcNot; }
            set
            {
                if (srcNot == value) return;

                srcNot = value;
                OnPropertyChanged("SrcNot");

            }
        }

        public DestinationFolder RefFound
        {
            get { return refFound; }
            set
            {
                if (refFound == value) return;

                refFound = value;
                OnPropertyChanged("ReferenceFound");
            }
        }

        public Searcher Searcher { get; private set; }

        public string Title => title;

        public string CompleteTitle => title;

        public ViewModelSearch()
        {
            Src = new Folder("", SubfolderType.This);
            Ref = new Folder("", SubfolderType.This);

            RefFound = new DestinationFolder()
            {
                IsDo = false,
                IsAllDelete = false,
                IsCopy = true,
                Dest = new Folder("", SubfolderType.This)
            };

            SrcFound = new DestinationFolder()
            {
                IsDo = false,
                IsAllDelete = false,
                IsCopy = true,
                Dest = new Folder("", SubfolderType.This)
            };

            SrcNot = new DestinationFolder()
            {
                IsDo = false,
                IsAllDelete = false,
                IsCopy = true,
                Dest = new Folder("", SubfolderType.This)
            };

            Searcher = new Searcher(this);
        }
    }
}
