using FolderFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProgram
{
    public class ViewModelMix : INotifyPropertyChanged, ITitle
    {
        private const string title = "Mischen";

        private bool isMix = true, isAuto = true;
        private Folder folder;

        public bool IsAuto
        {
            get { return isAuto; }
            set
            {
                if (isAuto == value) return;

                isAuto = value;
                OnPropertyChanged("IsAuto");
            }
        }

        public bool IsForce
        {
            get { return !isAuto; }
            set
            {
                if (isAuto != value) return;

                isAuto = !value;
                OnPropertyChanged("IsForce");
            }
        }

        public bool IsMix
        {
            get { return isMix; }
            set
            {
                if (isMix == value) return;

                isMix = value;
                OnPropertyChanged("IsMix");
            }
        }

        public bool IsDemix
        {
            get { return !isMix; }
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
            get { return folder; }
            set
            {
                if (folder == value) return;

                folder = value;
                OnPropertyChanged("Folder");
            }
        }

        public Mixer Mixer { get; private set; }

        public string Title => title;

        public string CompleteTitle => title;

        public ViewModelMix()
        {
            Folder = new Folder("", SubfolderType.This);

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
