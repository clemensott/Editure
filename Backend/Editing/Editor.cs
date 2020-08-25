using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Editure.Backend.Doer;
using Editure.Backend.Editing.EditEncoders;
using Editure.Backend.Editing.PictureEditing;
using Editure.Backend.Editing.ReferencePosition;
using Editure.Backend.ViewModels;
using FolderFile;

namespace Editure.Backend.Editing
{
    public class Editor : Doer<ViewModelEdit, FileInfo>
    {
        private bool isMoving, referencePointBottom, referencePointRight;
        private IntPoint moveBeginPoint, moveBeginOffset;

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

        protected override Task Do(FileInfo src)
        {
            try
            {
                string destPath = Path.Combine(viewModel.Dest.FullName, src.Name);
                EditImage img = viewModel.CreateEditImage(src.FullName);

                SavePicture(img.GetEditBitmap(), destPath);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Editor.Do");
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            
            return Task.CompletedTask;
        }

        public void SaveCurrentPicture()
        {
            FileInfo currentFile = viewModel.GetCurrentPictureFileInfo();

            if (currentFile == null) return;

            string DestPath = Path.Combine(viewModel.Dest.FullName, currentFile.Name);
            IEncoderManager encoderManager = viewModel.GetEncoder(currentFile.Extension);

            SavePicture(viewModel.ShowImg, destPath, encoderManager);
        }

        public void SavePicture(BitmapSource img, string destPath)
        {
            IEncoderManager encoderManager = viewModel.GetEncoder(Path.GetExtension(destPath));

            SavePicture(img, destPath, encoderManager);
        }

        public static void SavePicture(BitmapSource img, string destPath, IEncoderManager encoderManager)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(destPath);
                destPath = Path.ChangeExtension(destPath, encoderManager.Extension);

                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                if (File.Exists(destPath)) File.Delete(destPath);

                encoderManager.Encoder.Frames.Add(BitmapFrame.Create(img));

                GC.Collect();

                using (var stream = new FileStream(destPath, FileMode.Create))
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

        public void BeginMove(IntPoint intPoint)
        {
            isMoving = true;
            moveBeginPoint = intPoint;
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
            int x = Convert.ToInt32((currentPoint.X - moveBeginPoint.X) * ratio * (reverseX ? 1 : -1));
            int y = Convert.ToInt32((currentPoint.Y - moveBeginPoint.Y) * ratio * (reverseY ? 1 : -1));

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
