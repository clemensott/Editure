using System.IO;

namespace MainProgram
{
    public class Searcher : Doer<ViewModelSearch, FileInfo>
    {
        private FileInfo[] referenceFiles;

        public Searcher(ViewModelSearch parent) : base(parent)
        {
        }

        protected override FileInfo[] Initialize()
        {
            FileInfo[] srcFiles = Utils.SetSubTypeAndRefresh(viewModel.Src, "Refresh Search Src failed");
            FileInfo[] refFiles = Utils.SetSubTypeAndRefresh(viewModel.Ref, "Refresh Search Reference failed");

            if (viewModel.SrcFound.IsDo) Utils.DeleteContent(viewModel.SrcFound, "Delete SrcFound Content failed");
            if (viewModel.SrcNot.IsDo) Utils.DeleteContent(viewModel.SrcNot, "Delete SrcNot Content failed");
            if (viewModel.RefFound.IsDo) Utils.DeleteContent(viewModel.RefFound, "Delete ReferenceFound Content failed");

            referenceFiles = refFiles;

            return srcFiles;
        }

        protected override void Do(FileInfo Src)
        {
            bool found = false;

            foreach (FileInfo refFile in referenceFiles)
            {
                if (CompareFileInfos(Src, refFile))
                {
                    if (viewModel.SrcFound.IsDo)
                    {
                        FileInfo Dest = new FileInfo(Path.Combine(viewModel.SrcFound.Dest.FullName, refFile.Name));
                        Utils.CopyMoveFile(Src, Dest, viewModel.SrcFound.IsCopy);
                    }

                    if (viewModel.RefFound.IsDo)
                    {
                        FileInfo Dest = new FileInfo(Path.Combine(viewModel.RefFound.Dest.FullName, refFile.Name));
                        Utils.CopyMoveFile(refFile, Dest, viewModel.RefFound.IsCopy);
                    }

                    found = true;
                    break;
                }
            }

            if (!found && viewModel.SrcNot.IsDo)
            {
                FileInfo Dest = new FileInfo(Path.Combine(viewModel.SrcNot.Dest.FullName, Src.Name));
                Utils.CopyMoveFile(Src, Dest, viewModel.SrcNot.IsCopy);
            }
        }

        private bool CompareFileInfos(FileInfo file1, FileInfo file2)
        {
            if (viewModel.IsWithExtension) return file1.Name == file2.Name;

            return GetFileNameWithoutExtention(file1) == GetFileNameWithoutExtention(file2);
        }

        private string GetFileNameWithoutExtention(FileInfo fileInfo)
        {
            if (fileInfo.Extension.Length == 0) return fileInfo.Name;

            return fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length);
        }
    }
}
