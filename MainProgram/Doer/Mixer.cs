using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MainProgram
{
    public class Mixer
    {
        private ViewModelMix viewModel;

        public Mixer(ViewModelMix parent)
        {
            viewModel = parent;
        }

        public void Mix()
        {
            Random ran = new Random();

            Needed.TryCatchWithMsgBox(viewModel.Folder.RefreshFolderAndFiles, "Refresh Mix Folder failed before Mixing");
            List<FileInfo> SrcFileInfos = viewModel.Folder.GetFiles().ToList();

            while (SrcFileInfos.Count > 0)
            {
                int index = ran.Next(SrcFileInfos.Count);

                FileInfo Dest = GetMixedFileInfo(SrcFileInfos[index]);

                try
                {
                    SrcFileInfos[index].MoveTo(Dest.FullName);
                }
                catch { }

                SrcFileInfos.RemoveAt(index);
            }
        }

        public void Demix()
        {
            Needed.TryCatchWithMsgBox(viewModel.Folder.RefreshFolderAndFiles, "Refresh Mix Folder failed before Demixing");

            foreach (FileInfo Src in viewModel.Folder.GetFiles())
            {
                FileInfo Dest = GetDemixedFileInfo(Src);

                try
                {
                    Src.MoveTo(Dest.FullName);
                }
                catch { }
            }
        }

        private FileInfo GetMixedFileInfo(FileInfo file)
        {
            if (IsMixed(file.Name)) return file;

            FileInfo Dest;

            do
            {
                Dest = new FileInfo(Path.Combine(file.DirectoryName, RandomPart() + file.Name));

            } while (Dest.Exists);

            return Dest;
        }

        private FileInfo GetDemixedFileInfo(FileInfo file)
        {
            if (IsMixed(file.Name)) return file;

            try
            {
                return new FileInfo(Path.Combine(file.DirectoryName, file.Name.Remove(0, 5)));
            }
            catch { }

            return file;
        }

        private bool IsMixed(string name)
        {
            if (name.Length <= 5) return false;

            if (name[0] != '_') return false;
            for (int i = 1; i < 4; i++) if (!IsLowerCharOrNumber(name[i])) return false;
            if (name[4] != '_') return false;

            return true;
        }

        private bool IsLowerCharOrNumber(char c)
        {
            if (char.IsLetter(c) && char.IsLower(c)) return true;
            if (char.IsDigit(c)) return true;

            return false;
        }

        private string RandomPart()
        {
            return "_" + Path.GetRandomFileName().Remove(3) + "_";
        }
    }
}
