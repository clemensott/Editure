using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Editure.Backend.Doer;
using Editure.Backend.TextToVal;
using FolderFile;

namespace Editure.Backend.ViewModels
{
    public class ViewModelCopy : INotifyPropertyChanged, ITitle
    {
        private const string title = "Copy";

        private Folder src, dest;
        private CurrentItemList<FileInfo> pictures;
        private PreloadImage images;
        private readonly DispatcherTimer timer;

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

        public Folder Dest
        {
            get => dest;
            set
            {
                if (dest == value) return;

                dest = value;
                OnPropertyChanged("Dest");
            }
        }

        public string CurrentPictureIndex
        {
            get => Pictures.CurrentIndexBase1;
            set
            {
                if (value == CurrentPictureIndex) return;

                Pictures.CurrentIndexBase1.String = value;
            }
        }

        public string AllPictureCountText => "/ " + Pictures.Count;

        public ImageSource PreviousImg => images?.Previous ?? new BitmapImage();

        public ImageSource ShowImg => images?.Show ?? new BitmapImage();

        public ImageSource NextImg => images?.Next ?? new BitmapImage();

        public CurrentItemList<FileInfo> Pictures
        {
            get => pictures ?? new CurrentItemList<FileInfo>(Utils.DefaultFileInfo);
            set
            {
                if (value == pictures) return;

                pictures = value;
                pictures.CurrentIndexBase0.ValueChangingEvent += CurrentPictureIndex_ValueChangingEvent;
                images = new PreloadImage(value);

                ApplyPictureIndex(pictures.CurrentIndexBase0.Value);

                OnPropertyChanged("Pictures");
                OnPropertyChanged("CurrentPictureIndex");
                OnPropertyChanged("AllPictureCountText");
            }
        }

        public Copier Copier { get; }

        public string Title => title;

        public string CompleteTitle => (images?.Show == null ? string.Empty : (images.ShowPath + " - ")) + Title;

        public ViewModelCopy()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;

            src = null;
            dest = null;

            Copier = new Copier(this);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            OnPropertyChanged("NextImg");
            OnPropertyChanged("PreviousImg");
        }

        private void CurrentPictureIndex_ValueChangingEvent(object sender, ValueChangingEventArgs<int> e)
        {
            e.Apply = ApplyPictureIndex(e.NewValue);

            OnPropertyChanged("Pictures");
            OnPropertyChanged("CurrentPictureIndex");
            OnPropertyChanged("AllPictureCountText");
        }

        private bool ApplyPictureIndex(int newIndex)
        {
            if (Pictures.Count == 0 && newIndex == -1) return true;

            int index = newIndex;

            while (Pictures.Count > 0)
            {
                if (images.TrySetShowImage(Pictures[index].FullName))
                {
                    UpdateShowImg();

                    if (index != newIndex) Pictures.CurrentIndexBase0.Value = index;

                    timer.Start();
                    return true;
                }

                Pictures.RemoveAt(index);

                if (Pictures.Count <= index) index = Pictures.Count - 1;
            }

            images.Clear();
            Pictures.CurrentIndexBase0.Value = -1;

            return false;
        }

        public void UpdateShowImgPath()
        {
            if (images != null && images.ShowPath != Pictures.CurrentItem.FullName)
            {
                ApplyPictureIndex(Pictures.CurrentIndexBase0.Value);
            }
        }

        public void UpdateShowImg()
        {
            OnPropertyChanged("ShowImg");
            OnPropertyChanged("CompleteTitle");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
