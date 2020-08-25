using System;
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

            viewModel.Pictures = new CurrentItemList<FileInfo>(files, Utils.DefaultFileInfo);
        }

        public void CopyCurrentPicture()
        {
            string destPath = Path.Combine(viewModel.Dest.FullName, viewModel.Pictures.CurrentItem.Name);

            try
            {
                if (File.Exists(destPath)) File.Delete(destPath);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Copier.CopyCurrentPicture1");
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            try
            {
                viewModel.Pictures.CurrentItem.CopyTo(destPath);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Copier.CopyCurrentPicture2");
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        public void DeleteCurrentPicture()
        {
            try
            {
                viewModel.Pictures.CurrentItem.Delete();
                viewModel.Pictures.RemoveCurrent();
                viewModel.UpdateShowImgPath();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Copier.DeleteCurrentPicture2");
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
    }
}
