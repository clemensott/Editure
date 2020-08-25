using System.IO;
using System.Threading.Tasks;
using Editure.Backend.CopyMove;
using Editure.Backend.ViewModels;
using StdOttStandard;

namespace Editure.Backend.Doer
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
            if (viewModel.RefFound.IsDo)
                Utils.DeleteContent(viewModel.RefFound, "Delete ReferenceFound Content failed");

            referenceFiles = refFiles;

            return srcFiles;
        }

        protected override async Task Do(FileInfo src)
        {
            bool found = false;
            Task srcLockObj = null, refLockObj = null, srcNotLockObj = null;

            foreach (FileInfo refFile in referenceFiles)
            {
                if (CompareFileInfos(src, refFile))
                {
                    if (viewModel.SrcFound.IsDo)
                    {
                        srcLockObj = CopyMoveFiles.Current.ToFolder(src,
                            viewModel.SrcFound.Dest.FullName, viewModel.SrcFound.IsCopy);
                    }

                    if (viewModel.RefFound.IsDo)
                    {
                        refLockObj = CopyMoveFiles.Current.ToFolder(refFile,
                            viewModel.RefFound.Dest.FullName, viewModel.RefFound.IsCopy);
                    }

                    found = true;
                    break;
                }
            }

            if (!found && viewModel.SrcNot.IsDo)
            {
                srcNotLockObj = CopyMoveFiles.Current.ToFolder(src,
                    viewModel.SrcNot.Dest.FullName, viewModel.SrcNot.IsCopy);
            }

            await srcLockObj.ToNotNull();
            await refLockObj.ToNotNull();
            await srcNotLockObj.ToNotNull();
        }

        private bool CompareFileInfos(FileSystemInfo file1, FileSystemInfo file2)
        {
            if (viewModel.IsWithExtension) return file1.Name == file2.Name;

            return Path.GetFileNameWithoutExtension(file1.Name) == Path.GetFileNameWithoutExtension(file2.Name);
        }
    }
}