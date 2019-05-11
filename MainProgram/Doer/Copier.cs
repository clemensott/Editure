using System;
using System.Collections.ObjectModel;
using System.IO;

namespace MainProgram
{
    public class Copier
    {
        private ViewModelCopy viewModel;

        public Copier(ViewModelCopy parent)
        {
            viewModel = parent;
        }

        public void Open()
        {
            FileInfo[] files = Utils.SetSubTypeAndRefresh(viewModel.Src, "Refresh Copy Src failed");
            viewModel.Pictures = new ObservableCollection<FileInfo>(files);
        }

        public void CopyCurrentPicture()
        {
            FileInfo currentFile = viewModel.GetCurrentPictureFileInfo();

            if (currentFile == null) return;

            string DestPath = Path.Combine(viewModel.Dest.FullName, currentFile.Name);

            try
            {
                if (File.Exists(DestPath)) File.Delete(DestPath);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Copier.CopyCurrentPicture1");
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            try
            {
                currentFile.CopyTo(DestPath);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Copier.CopyCurrentPicture2");
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        public async void DeleteCurrentPicture()
        {
            try
            {
                FileInfo currentFile = viewModel.GetCurrentPictureFileInfo();

                currentFile?.Delete();
                viewModel.Pictures.Remove(currentFile);
                await viewModel.UpdateShowImgPathAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Copier.DeleteCurrentPicture2");
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
    }
}
