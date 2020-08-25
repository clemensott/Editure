using System;
using System.Collections.ObjectModel;
using System.IO;
using Editure.Backend.ViewModels;

namespace Editure.Backend.Doer
{
    public class Copier
    {
        private readonly ViewModelCopy viewModel;

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
            FolderFile.Folder destFolder = viewModel.Dest;

            if (currentFile == null || string.IsNullOrWhiteSpace(destFolder?.FullName)) return;

            string destPath = Path.Combine(destFolder.FullName, currentFile.Name);

            try
            {
                currentFile.CopyTo(destPath, true);
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
