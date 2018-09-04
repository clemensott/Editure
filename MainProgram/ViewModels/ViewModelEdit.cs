using FolderFile;
using System.IO;
using System.Windows.Media.Imaging;

namespace MainProgram
{
    public class ViewModelEdit : ViewModelPauseable, ITitle
    {
        private const string title = "Bearbeiten";

        private Folder src, dest;
        private CurrentItemList<FileInfo> pictures;
        private EditPictureProperties properties;
        private EditImage editImg;
        private EditEncoderType encoderType;

        public bool IsImgLoaded { get { return editImg != null; } }

        public bool IsFlipX
        {
            get { return (editImg?.Properties ?? properties).FlipX; }
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
            get { return (editImg?.Properties ?? properties).FlipY; }
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

        public string CurrentPictureIndex
        {
            get { return Pictures.CurrentIndexBase1; }
            set
            {
                if (value == CurrentPictureIndex) return;

                Pictures.CurrentIndexBase1.String = value;
            }
        }

        public string AllPictureCountText { get { return "/ " + Pictures.Count; } }

        public IntSize ScaleSize { get { return editImg?.Properties.GetScale(OriginalSize) ?? IntSize.Empty; } }

        public IntSize Wanna
        {
            get { return (editImg?.Properties ?? properties).Wanna; }
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
            get { return (editImg?.Properties ?? properties).GetRelativeOffset(OriginalSize); }
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
            get { return (editImg?.Properties ?? properties).ModeType; }
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
            get { return (editImg?.Properties ?? properties).ReferencePositionType; }
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

        public IEncoderManager CurrentPictureEncoder
        {
            get { return GetEncoder(Pictures.CurrentItem.Extension); }
        }

        public CurrentItemList<FileInfo> Pictures
        {
            get { return pictures ?? new CurrentItemList<FileInfo>(Needed.DefaultFileInfo); }
            set
            {
                if (value == pictures) return;

                pictures = value;
                pictures.CurrentIndexBase0.ValueChangingEvent += CurrentPictureIndex_ValueChangingEvent;

                ApplyPictureIndex(pictures.CurrentIndexBase0.Value);
                UpdatePictures();
                UpdateShowImg();
            }
        }

        public Editor Editor { get; private set; }

        public string Title => title;

        public string CompleteTitle
        {
            get
            {
                return ((pictures?.Count ?? 0) == 0 ? string.Empty : pictures.CurrentItem.Name + " - ") + Title;
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

            Src = new Folder("", SubfolderType.This);
            Dest = new Folder("", SubfolderType.This);

            Pictures = new CurrentItemList<FileInfo>(Needed.DefaultFileInfo);

            Editor = new Editor(this);
        }

        private void CurrentPictureIndex_ValueChangingEvent(object sender, ValueChangingEventArgs<int> e)
        {
            e.Apply = ApplyPictureIndex(e.NewValue);

            UpdatePictures();
            UpdateOffset();
            UpdateShowImg();
        }

        private bool ApplyPictureIndex(int newIndex)
        {
            if (Pictures.Count == 0 && newIndex == -1) return true;

            int index = newIndex;

            while (Pictures.Count > 0)
            {
                try
                {
                    editImg = CreateEditImage(Pictures[index].FullName);

                    if (index != newIndex) Pictures.CurrentIndexBase0.Value = index;

                    return true;
                }
                catch { }

                Pictures.RemoveAt(index);

                if (index >= Pictures.Count) index = Pictures.Count - 1;
            }

            Pictures.CurrentIndexBase0.Value = -1;

            if (editImg != null)
            {
                properties = editImg.Properties;
                editImg = null;
            }

            return false;
        }

        public EditImage CreateEditImage(string path)
        {
            return new EditImage(path, Wanna, ModeType,
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
