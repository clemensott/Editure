using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            Needed.TryCatchWithMsgBox(viewModel.Src.RefreshFolderAndFiles, "Refresh Search Src failed");
            Needed.TryCatchWithMsgBox(viewModel.Ref.RefreshFolderAndFiles, "Refresh Search Reference failed");

            if (viewModel.SrcFound.IsDo && viewModel.SrcFound.IsAllDelete && viewModel.SrcFound.Dest.Info.Exists)
            {
                Needed.TryCatchWithMsgBox(viewModel.SrcFound.Dest.DeleteContent, "Delete SrcFound Content failed");
            }

            if (viewModel.SrcNot.IsDo && viewModel.SrcNot.IsAllDelete && viewModel.SrcNot.Dest.Info.Exists)
            {
                Needed.TryCatchWithMsgBox(viewModel.SrcNot.Dest.DeleteContent, "Delete SrcNot Content failed");
            }

            if (viewModel.RefFound.IsDo &&viewModel.RefFound.IsAllDelete&& viewModel.RefFound.Dest.Info.Exists)
            {
                Needed.TryCatchWithMsgBox(viewModel.RefFound.Dest.DeleteContent, "Delete ReferenceFound Content failed");
            }

            referenceFiles = viewModel.Ref.GetFiles();

            return viewModel.Src.GetFiles().ToArray();
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
                        FileInfo Dest = new FileInfo(Path.Combine(viewModel.SrcFound.Dest.FullPath, refFile.Name));
                        Needed.CopyMoveFile(Src, Dest, viewModel.SrcFound.IsCopy);
                    }

                    if (viewModel.RefFound.IsDo)
                    {
                        FileInfo Dest = new FileInfo(Path.Combine(viewModel.RefFound.Dest.FullPath, refFile.Name));
                        Needed.CopyMoveFile(refFile, Dest, viewModel.RefFound.IsCopy);
                    }

                    found = true;
                    break;
                }
            }

            if (!found && viewModel.SrcNot.IsDo)
            {
                FileInfo Dest = new FileInfo(Path.Combine(viewModel.SrcNot.Dest.FullPath, Src.Name));
                Needed.CopyMoveFile(Src, Dest, viewModel.SrcNot.IsCopy);
            }

            //viewModel.ThreadHandler.Release();
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
