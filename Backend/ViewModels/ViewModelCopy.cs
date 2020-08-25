using FolderFile;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Editure.Backend.Doer;
using StdOttStandard;

namespace Editure.Backend.ViewModels
{
    public class ViewModelCopy : INotifyPropertyChanged, ITitle
    {
        private const string title = "Copy";

        private Folder src, dest;
        private bool isLoadingCurrentImage;
        private int latestCurrentIndex, setCurrentIndexOffset, currentPictureIndex1;
        private ObservableCollection<FileInfo> pictures;
        private PreloadImage images;

        public bool IsLoadingCurrentImage
        {
            get { return isLoadingCurrentImage; }
            set
            {
                if (value == isLoadingCurrentImage) return;

                isLoadingCurrentImage = value;
                OnPropertyChanged(nameof(IsLoadingCurrentImage));
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

        public int CurrentPictureIndex1
        {
            get => currentPictureIndex1;
            set => SetPossibleCurrentPictureIndexAsync(value, 0);
        }

        public ImageSource PreviousImg => images?.Previous;

        public ImageSource ShowImg => images?.Current;

        public ImageSource NextImg => images?.Next;

        public ObservableCollection<FileInfo> Pictures
        {
            get => pictures;
            set
            {
                if (value == pictures) return;

                pictures = value;
                OnPropertyChanged("Pictures");

                images = new PreloadImage(value);
                CurrentPictureIndex1 = 1;
            }
        }

        public Copier Copier { get; }

        public string Title => title;

        public string CompleteTitle => (images?.Current == null ? string.Empty : (images.CurrentPath + " - ")) + Title;

        public ViewModelCopy()
        {
            src = null;
            dest = null;

            Copier = new Copier(this);
            Pictures = new ObservableCollection<FileInfo>();
            CurrentPictureIndex1 = 0;
        }

        private void SetLatestCurrentIndex(int index)
        {
            latestCurrentIndex = StdUtils.CycleIndex(index, Pictures.Count, 1);
        }

        public async Task SetPreviousPictureAsync()
        {
            await SetPossibleCurrentPictureIndexAsync(CurrentPictureIndex1, -1);
        }

        public async Task SetNextPictureAsync()
        {
            await SetPossibleCurrentPictureIndexAsync(CurrentPictureIndex1, 1);
        }

        public async Task SetPossibleCurrentPictureIndexAsync()
        {
            await SetPossibleCurrentPictureIndexAsync(CurrentPictureIndex1, 0);
        }

        private async Task SetPossibleCurrentPictureIndexAsync(int index, int offset)
        {
            latestCurrentIndex = index;
            setCurrentIndexOffset = offset;

            if (IsLoadingCurrentImage) return;
            IsLoadingCurrentImage = true;

            do
            {
                SetLatestCurrentIndex(latestCurrentIndex + setCurrentIndexOffset);
            }
            while (!await TrySetCurrentPictureIndex1(latestCurrentIndex));

            IsLoadingCurrentImage = false;
        }

        private async Task<bool> TrySetCurrentPictureIndex1(int value)
        {
            if (Pictures.Count == 0)
            {
                SetCurrentPictureIndex1(0);
                return value == 0;
            }

            string path = GetPictureFileInfo(value)?.FullName;
            BitmapImage img = await images.TryGetImageAsync(path);

            if (img != null)
            {
                images.SetShowImage(path, img);
                SetCurrentPictureIndex1(value);

                LoadNextNadPreviousPicture();
                return true;
            }

            Pictures.RemoveAt(value - 1);

            return false;
        }

        private void SetCurrentPictureIndex1(int value)
        {
            currentPictureIndex1 = value;
            OnPropertyChanged(nameof(CurrentPictureIndex1));

            UpdateShowImg();
        }

        public FileInfo GetCurrentPictureFileInfo()
        {
            return GetPictureFileInfo(CurrentPictureIndex1);
        }

        public FileInfo GetPictureFileInfo(int indexBase1)
        {
            return Pictures?.ElementAtOrDefault(indexBase1 - 1);
        }

        private async void LoadNextNadPreviousPicture()
        {
            await Task.Delay(200);

            await images.UpdateOtherImages();

            OnPropertyChanged("NextImg");
            OnPropertyChanged("PreviousImg");
        }

        public async Task UpdateShowImgPathAsync()
        {
            if (images != null && images.CurrentPath != GetCurrentPictureFileInfo()?.FullName)
            {
                await SetPossibleCurrentPictureIndexAsync();
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
