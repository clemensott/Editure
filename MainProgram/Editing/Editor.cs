using FolderFile;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace MainProgram
{
    public class Editor : Doer<ViewModelEdit, FileInfo>
    {
        private bool isMoving, referencePointBottom, referencePointRight;
        private IntPoint movebeginPoint, moveBeginOffset;

        public Editor(ViewModelEdit parent) : base(parent)
        {
        }

        public void Open()
        {
            FileInfo[] files;

            if (viewModel.Src.SubType == SubfolderType.No)
            {
                Utils.TryCatchWithMsgBox(() => viewModel.Src.SubType = SubfolderType.This, "Refresh Edit Src failed");
                files = viewModel.Src.Files;
            }
            else files = Utils.TryCatchWithMsgBox(viewModel.Src.Refresh, "Refresh Edit Src failed");

            viewModel.Pictures = new ObservableCollection<FileInfo>(files);
        }

        protected override FileInfo[] Initialize()
        {
            return Utils.TryCatchWithMsgBox(viewModel.Src.Refresh, "Refresh Edit Src failed");
        }

        protected override void Do(FileInfo Src)
        {
            try
            {
                string DestPath = Path.Combine(viewModel.Dest.FullName, Src.Name);
                EditImage img = viewModel.CreateEditImage(Src.FullName);

                SavePicture(img.GetEditBitmap(), DestPath);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Editor.Do");
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        public void SaveCurrentPicture()
        {
            FileInfo currentFile = viewModel.GetCurrentPictureFileInfo();

            if (currentFile == null) return;

            string DestPath = Path.Combine(viewModel.Dest.FullName, currentFile.Name);
            IEncoderManager encoderManager = viewModel.GetEncoder(currentFile.Extension);

            SavePicture(viewModel.ShowImg, DestPath, encoderManager);
        }

        public void SavePicture(BitmapSource img, string DestPath)
        {
            IEncoderManager encoderManager = viewModel.GetEncoder(Path.GetExtension(DestPath));

            SavePicture(img, DestPath, encoderManager);
        }

        public void SavePicture(BitmapSource img, string DestPath, IEncoderManager encoderManager)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(DestPath);
                DestPath = Path.ChangeExtension(DestPath, encoderManager.Extention);

                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                if (File.Exists(DestPath)) File.Delete(DestPath);

                encoderManager.Encoder.Frames.Add(BitmapFrame.Create(img));

                GC.Collect();

                using (var stream = new FileStream(DestPath, FileMode.Create))
                {
                    encoderManager.Encoder.Save(stream);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Editor.SavePicture");
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        public void BeginMove(IntPoint IntPoint)
        {
            isMoving = true;
            movebeginPoint = IntPoint;
            moveBeginOffset = viewModel.Offset;

            EditReferencePositionType referencePoint = viewModel.ReferencePosition;

            referencePointBottom = referencePoint == EditReferencePositionType.BottomLeft ||
                referencePoint == EditReferencePositionType.BottomCenter ||
                referencePoint == EditReferencePositionType.BottomRight;

            referencePointRight = referencePoint == EditReferencePositionType.TopRight ||
                referencePoint == EditReferencePositionType.CenterRight ||
                referencePoint == EditReferencePositionType.BottomRight;
        }

        public void Move(IntPoint currentPoint, double controlWidth)
        {
            double ratio;
            IntPoint offset;

            if (!viewModel.IsImgLoaded || !isMoving)
            {
                isMoving = false;
                return;
            }

            ratio = viewModel.Wanna.Width / controlWidth;
            offset = GetMovedOffset(currentPoint, ratio, referencePointRight, referencePointBottom);
            offset.Offset(moveBeginOffset);

            viewModel.Offset = offset;
        }

        private IntPoint GetMovedOffset(IntPoint currentPoint, double ratio, bool reverseX, bool reverseY)
        {
            int x = Convert.ToInt32((currentPoint.X - movebeginPoint.X) * ratio * (reverseX ? 1 : -1));
            int y = Convert.ToInt32((currentPoint.Y - movebeginPoint.Y) * ratio * (reverseY ? 1 : -1));

            return new IntPoint(x, y);
        }

        public void Move(IntPoint offsetToOffset)
        {
            IntPoint offset = viewModel.Offset;

            offset.Offset(offsetToOffset);

            viewModel.Offset = offset;
        }

        public void EndMove()
        {
            isMoving = false;
        }
    }
}
