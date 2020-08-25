using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editure.Backend.ViewModels;

namespace Editure.Backend.Doer
{
    public class Mixer
    {
        private static readonly Random ran = new Random();
        private readonly ViewModelMix viewModel;

        public Mixer(ViewModelMix parent)
        {
            viewModel = parent;
        }

        public void Mix()
        {
            string message = "Refresh Mix Folder failed before Mixing";
            List<FileInfo> srcFileInfos = Utils.SetSubTypeAndRefresh(viewModel.Folder, message)?.ToList();

            if (srcFileInfos == null) return;

            while (srcFileInfos.Count > 0)
            {
                int index = ran.Next(srcFileInfos.Count);

                FileInfo dest = GetMixedFileInfo(srcFileInfos[index]);

                try
                {
                    srcFileInfos[index].MoveTo(dest.FullName);
                }
                catch { }

                srcFileInfos.RemoveAt(index);
            }
        }

        public void Demix()
        {
            const string message = "Refresh Mix Folder failed before Demixing";
            FileInfo[] files = Utils.SetSubTypeAndRefresh(viewModel.Folder, message);

            foreach (FileInfo src in files ?? Enumerable.Empty<FileInfo>())
            {
                FileInfo dest = GetDemixedFileInfo(src);

                try
                {
                    src.MoveTo(dest.FullName);
                }
                catch { }
            }
        }

        private static FileInfo GetMixedFileInfo(FileInfo file)
        {
            if (IsMixed(file.Name)) return file;

            FileInfo dest;

            do
            {
                dest = new FileInfo(Path.Combine(file.DirectoryName, RandomPart() + file.Name));

            } while (dest.Exists);

            return dest;
        }

        private static FileInfo GetDemixedFileInfo(FileInfo file)
        {
            if (!IsMixed(file.Name)) return file;

            try
            {
                return new FileInfo(Path.Combine(file.DirectoryName, file.Name.Remove(0, 5)));
            }
            catch { }

            return file;
        }

        private static bool IsMixed(string name)
        {
            if (name.Length <= 5) return false;

            if (name[0] != '_') return false;
            for (int i = 1; i < 4; i++) if (!IsLowerCharOrNumber(name[i])) return false;
            if (name[4] != '_') return false;

            return true;
        }

        private static bool IsLowerCharOrNumber(char c)
        {
            if (char.IsLetter(c) && char.IsLower(c)) return true;
            if (char.IsDigit(c)) return true;

            return false;
        }

        private static string RandomPart()
        {
            return "_" + Path.GetRandomFileName().Remove(3) + "_";
        }
    }
}
