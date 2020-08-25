using System.IO;
using System.Windows.Media.Imaging;
using Editure.Backend.Editing;
using Editure.Backend.Editing.EditEncoders;
using Editure.Backend.Editing.EditMode;
using Editure.Backend.Editing.PictureEditing;
using Editure.Backend.Editing.ReferencePosition;
using Editure.Backend.TextToVal;
using FolderFile;

namespace Editure.Backend.ViewModels
{
    public class ViewModelEdit : ViewModelPauseable, ITitle
    {
        private const string title = "Edit";

        private Folder src, dest;
        private CurrentItemList<FileInfo> pictures;
        private EditPictureProperties properties;
        private EditImage editImg;
        private EditEncoderType encoderType;

        public bool IsImgLoaded => editImg != null;

        public bool IsFlipX
        {
            get => (editImg?.Properties ?? properties).FlipX;
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
            get => (editImg?.Properties ?? properties).FlipY;
            set
            {
                if (value == IsFlipY) return;

                if (editImg != null) editImg.Properties.FlipY = value;
                else properties.FlipY = value;

                OnPropertyChanged("IsFlipY");
                UpdateShowImg();
            }
        }

        public IntSize OriginalSize => editImg?.OriginalSize ?? IntSize.Empty;

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

        public IntSize ScaleSize => editImg?.Properties.GetScale(OriginalSize) ?? IntSize.Empty;

        public IntSize Wanna
        {
            get => (editImg?.Properties ?? properties).Wanna;
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
            get => (editImg?.Properties ?? properties).GetRelativeOffset(OriginalSize);
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
            get => (editImg?.Properties ?? properties).ModeType;
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
            get => (editImg?.Properties ?? properties).ReferencePositionType;
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
            get => encoderType;
            set
            {
                if (value == encoderType) return;

                encoderType = value;
                OnPropertyChanged("EncoderType");
            }
        }

        public IEncoderManager CurrentPictureEncoder => GetEncoder(Pictures.CurrentItem.Extension);

        public CurrentItemList<FileInfo> Pictures
        {
            get => pictures ?? new CurrentItemList<FileInfo>(Utils.DefaultFileInfo);
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

        public Editor Editor { get; }

        public string Title => title;

        public string CompleteTitle => ((pictures?.Count ?? 0) == 0 ? string.Empty : pictures.CurrentItem.Name + " - ") + Title;

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

            Pictures = new CurrentItemList<FileInfo>(Utils.DefaultFileInfo);

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

        public IEncoderManager GetEncoder(string extension)
        {
            switch (encoderType)
            {
                case EditEncoderType.Auto:
                    return new EncoderManagerAuto(extension);

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
