using System;
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
            Needed.TryCatchWithMsgBox(viewModel.Src.RefreshFolderAndFiles, "Refresh Copy Src failed");

            viewModel.Pictures = new CurrentItemList<FileInfo>(viewModel.Src.GetFiles(), Needed.DefaultFileInfo);
        }

        public void CopyCurrentPicture()
        {
            string DestPath = Path.Combine(viewModel.Dest.FullPath, viewModel.Pictures.CurrentItem.Name);

            try
            {
                if (File.Exists(DestPath)) File.Delete(DestPath);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Copier.CopyCurrentPicture1");
                System.Diagnostics.Debug.WriteLine(Needed.GetMessage(e));
            }

            try
            {
                viewModel.Pictures.CurrentItem.CopyTo(DestPath);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Copier.CopyCurrentPicture2");
                System.Diagnostics.Debug.WriteLine(Needed.GetMessage(e));
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
                System.Diagnostics.Debug.WriteLine(Needed.GetMessage(e));
            }
        }
    }
}
