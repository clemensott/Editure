using FolderFile;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MainProgram
{
    public class ViewModelEdit : ViewModelPauseable, ITitle
    {
        private const string title = "Bearbeiten";

        private Folder src, dest;
        private bool isLoadingCurrentImage;
        private int latestCurrentIndex, setCurrentIndexOffset, currentPictureIndex1;
        private ObservableCollection<FileInfo> pictures;
        private EditPictureProperties properties;
        private EditImage editImg;
        private EditEncoderType encoderType;

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

        public bool IsImgLoaded { get { return editImg != null; } }

        public bool IsFlipX
        {
            get { return Properties.FlipX; }
            set
            {
                if (value == IsFlipX) return;

                if (editImg != null) editImg.Properties.FlipX = value;
                else properties.FlipX = value;

                OnPropertyChanged("IsFlipX");
                UpdateShowImg();
            }
        }

        public bool IsFlipY
        {
            get { return Properties.FlipY; }
            set
            {
                if (value == IsFlipY) return;

                if (editImg != null) editImg.Properties.FlipY = value;
                else properties.FlipY = value;

                OnPropertyChanged("IsFlipY");
                UpdateShowImg();
            }
        }

        public IntSize OriginalSize { get { return editImg?.OriginalSize ?? IntSize.Empty; } }

        public int CurrentPictureIndex1
        {
            get { return currentPictureIndex1; }
            set { SetPossibleCurrentPictureIndexAsync(value, 0); }
        }

        public string AllPictureCountText { get { return "/ " + Pictures.Count; } }

        public IntSize ScaleSize { get { return editImg?.Properties.GetScale(OriginalSize) ?? IntSize.Empty; } }

        public IntSize Wanna
        {
            get { return Properties.Wanna; }
            set
            {
                if (value == Wanna) return;

                if (editImg != null) editImg.Properties.Wanna = value;
                else properties.Wanna = value;

                UpdateWanna();
                UpdateShowImg();
            }
        }

        public IntPoint Offset
        {
            get { return Properties.GetRelativeOffset(OriginalSize); }
            set
            {
                if (Offset == value) return;

                if (editImg != null) editImg.Properties.SetRelativeOffset(value);
                else properties.SetRelativeOffset(value);

                UpdateOffset();
                UpdateShowImg();
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

        public Folder Dest
        {
            get { return dest; }
            set
            {
                if (dest == value) return;

                dest = value;
                OnPropertyChanged("Dest");
            }
        }

        public EditPictureProperties Properties { get { return editImg?.Properties ?? properties; } }

        public BitmapSource ShowImg
        {
            get
            {
                try
                {
                    if (editImg != null) return editImg.GetEditBitmap();
                }
                catch { }

                return new BitmapImage();
            }
        }

        public EditMode ModeType
        {
            get { return Properties.ModeType; }
            set
            {
                if (value == ModeType) return;

                if (editImg != null) editImg.Properties.ModeType = value;
                else properties.ModeType = value;

                OnPropertyChanged("ModeType");
                UpdateShowImg();
            }
        }

        public EditReferencePositionType ReferencePosition
        {
            get { return Properties.ReferencePositionType; }
            set
            {
                if (value == ReferencePosition) return;

                if (editImg != null) editImg.Properties.ReferencePositionType = value;
                else properties.ReferencePositionType = value;

                OnPropertyChanged("ReferencePosition");
                UpdateShowImg();
            }
        }

        public EditEncoderType EncoderType
        {
            get { return encoderType; }
            set
            {
                if (value == encoderType) return;

                encoderType = value;
                OnPropertyChanged("EncoderType");
            }
        }

        public ObservableCollection<FileInfo> Pictures
        {
            get { return pictures; }
            set
            {
                if (value == pictures) return;

                pictures = value;
                OnPropertyChanged(nameof(Pictures));

                CurrentPictureIndex1 = 1;
            }
        }

        public Editor Editor { get; private set; }

        public string Title => title;

        public string CompleteTitle
        {
            get
            {
                FileInfo currentFile = GetCurrentPictureFileInfo();

                return currentFile == null ? Title : (currentFile.Name + " - " + Title);
            }
        }

        public ViewModelEdit()
        {
            IsFlipX = false;
            IsFlipY = false;

            Wanna = new IntSize(1920, 1080);
            Offset = new IntPoint(0, 0);

            EncoderType = EditEncoderType.Auto;
            ModeType = EditMode.Crop;
            ReferencePosition = EditReferencePositionType.CenterCenter;

            Src = null;
            Dest = null;

            Pictures = new ObservableCollection<FileInfo>();

            Editor = new Editor(this);
        }

        private void SetLatestCurrentIndex(int index)
        {
            latestCurrentIndex = StdOttStandard.Utils.CycleIndex(index, Pictures.Count, 1);
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

            try
            {
                byte[] bytes = await Task.Run(() => File.ReadAllBytes(GetPictureFileInfo(value).FullName));
                editImg = CreateEditImage(bytes);
                SetCurrentPictureIndex1(value);

                return true;
            }
            catch { }

            Pictures.RemoveAt(value - 1);

            if (editImg != null)
            {
                properties = editImg.Properties;
                editImg = null;
            }

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

        public EditImage CreateEditImage(string path)
        {
            return new EditImage(path, Wanna, ModeType,
                IsFlipX, IsFlipY, Offset, ReferencePosition);
        }

        public EditImage CreateEditImage(byte[] bytes)
        {
            return new EditImage(bytes, Wanna, ModeType,
                IsFlipX, IsFlipY, Offset, ReferencePosition);
        }

        public IEncoderManager GetEncoder(string extention)
        {
            switch (encoderType)
            {
                case EditEncoderType.Auto:
                    return new EncoderManagerAuto(extention);

                case EditEncoderType.Bmp:
                    return new EncoderManagerBmp();

                case EditEncoderType.Jpg:
                    return new EncoderManagerJpg();

                case EditEncoderType.Png:
                    return new EncoderManagerPng();

                case EditEncoderType.Gif:
                    return new EncoderManagerGif();

                case EditEncoderType.Tiff:
                    return new EncoderManagerTiff();
            }

            return new EncoderManagerBmp();
        }

        private void UpdateOffset()
        {
            OnPropertyChanged("Offset");
        }

        private void UpdateWanna()
        {
            OnPropertyChanged("Wanna");

            UpdateSizes();
        }

        private void UpdateSizes()
        {
            OnPropertyChanged("OriginalSize");
            OnPropertyChanged("ScaleSize");
        }

        private void UpdatePictures()
        {
            OnPropertyChanged("Pictures");
            OnPropertyChanged("CurrentPictureIndex");
            OnPropertyChanged("AllPictureCountText");

            UpdateSizes();
        }

        public void UpdateShowImg()
        {
            OnPropertyChanged("ShowImg");
            OnPropertyChanged("CompleteTitle");
        }
    }
}
