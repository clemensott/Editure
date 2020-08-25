using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using FolderFile;

namespace Editure.Backend
{
    public class DestinationFolder : INotifyPropertyChanged
    {
        private bool isDo, isAllDelete, isMove;
        private Folder dest;

        public bool IsDo
        {
            get => isDo;
            set
            {
                if (value == isDo) return;

                isDo = value;
                OnPropertyChanged("IsDo");
            }
        }

        public bool IsAllDelete
        {
            get => isAllDelete;
            set
            {
                if (value == isAllDelete) return;

                isAllDelete = value;
                OnPropertyChanged("IsAllDelete");
            }
        }

        public bool IsMove
        {
            get => isMove;
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
            get => !isMove;
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
            get => dest;
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
            get => Dest?.FullName ?? string.Empty;
            set => Dest = value != DestPath && Directory.Exists(value) ? new Folder(value, SubfolderType.This) : null;
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
