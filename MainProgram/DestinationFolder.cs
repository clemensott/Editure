using FolderFile;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace MainProgram
{
    public class DestinationFolder : INotifyPropertyChanged
    {
        private bool isDo, isAllDelete, isMove;
        private Folder dest;

        public bool IsDo
        {
            get { return isDo; }
            set
            {
                if (value == isDo) return;

                isDo = value;
                OnPropertyChanged("IsDo");
            }
        }

        public bool IsAllDelete
        {
            get { return isAllDelete; }
            set
            {
                if (value == isAllDelete) return;

                isAllDelete = value;
                OnPropertyChanged("IsAllDelete");
            }
        }

        public bool IsMove
        {
            get { return isMove; }
            set
            {
                if (value == isMove) return;

                isMove = value;
                OnPropertyChanged("IsMove");
                OnPropertyChanged("IsCopy");
            }
        }

        public bool IsCopy
        {
            get { return !isMove; }
            set
            {
                if (value == IsCopy) return;

                isMove = !value;
                OnPropertyChanged("IsMove");
                OnPropertyChanged("IsCopy");
            }
        }

        [XmlIgnore]
        public Folder Dest
        {
            get { return dest; }
            set
            {
                if (dest == value) return;

                dest = value;
                OnPropertyChanged("Dest");
                OnPropertyChanged("DestPath");
            }
        }

        public string DestPath
        {
            get { return Dest?.FullName ?? string.Empty; }
            set { if (value != DestPath && Directory.Exists(value)) Dest = new Folder(value, SubfolderType.This); }
        }

        public DestinationFolder()
        {
            IsDo = false;
            IsAllDelete = false;
            IsCopy = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
