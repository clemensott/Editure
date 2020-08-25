using System.ComponentModel;
using Editure.Backend.Doer;
using FolderFile;

namespace Editure.Backend.ViewModels
{
    public class ViewModelMix : INotifyPropertyChanged, ITitle
    {
        private const string title = "Mix";

        private bool isMix = true, isAuto = true;
        private Folder folder;

        public bool IsAuto
        {
            get => isAuto;
            set
            {
                if (isAuto == value) return;

                isAuto = value;
                OnPropertyChanged("IsAuto");
            }
        }

        public bool IsForce
        {
            get => !isAuto;
            set
            {
                if (isAuto != value) return;

                isAuto = !value;
                OnPropertyChanged("IsForce");
            }
        }

        public bool IsMix
        {
            get => isMix;
            set
            {
                if (isMix == value) return;

                isMix = value;
                OnPropertyChanged("IsMix");
            }
        }

        public bool IsDemix
        {
            get => !isMix;
            set
            {
                if (isMix != value) return;

                isMix = !value;
                IsAuto = true;
                OnPropertyChanged("IsMix");
                OnPropertyChanged("IsDemix");
            }
        }

        public Folder Folder
        {
            get => folder;
            set
            {
                if (folder == value) return;

                folder = value;
                OnPropertyChanged("Folder");
            }
        }

        public Mixer Mixer { get; }

        public string Title => title;

        public string CompleteTitle => title;

        public ViewModelMix()
        {
            Folder = null;

            Mixer = new Mixer(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
