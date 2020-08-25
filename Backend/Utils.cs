using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using FolderFile;

namespace Editure.Backend
{
    public static class Utils
    {
        public static readonly FileInfo DefaultFileInfo = GetDefaultFileInfo();

        private static FileInfo GetDefaultFileInfo()
        {
            try
            {
                return new FileInfo("Editure.ico");
            }
            catch
            {
            }

            return null;
        }

        public static void TryCatchWithMsgBox(Action action, string name)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), name);
            }
        }

        public static T TryCatchWithMsgBox<T>(Func<T> action, string name)
        {
            try
            {
                return action();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), name);
                return default(T);
            }
        }

        public static FileInfo[] SetSubTypeAndRefresh(Folder folder, string errorMessage)
        {
            if (folder.SubType != SubfolderType.No) return TryCatchWithMsgBox(folder.Refresh, errorMessage);

            TryCatchWithMsgBox(() => folder.SubType = SubfolderType.This, errorMessage);
            return folder.Files;
        }

        public static void DeleteContent(DestinationFolder folder, string errorMessage)
        {
            if (folder.IsAllDelete && (folder.Dest?.GetDirectory()?.Exists ?? false))
            {
                TryCatchWithMsgBox(folder.Dest.DeleteContent, errorMessage);
            }
        }

        public static string GetFileInfoSize(FileInfo file)
        {
            string[] endings = new string[] {"B", "kB", "MB", "GB", "TB", "PB", "EB"};
            string ending = endings.Last();
            double value = Convert.ToDouble(file.Length);

            foreach (string e in endings)
            {
                ending = e;
                if (value < 1024) break;

                value /= 1024.0;
            }

            return Math.Round(value, 3).ToString(CultureInfo.CurrentCulture) + " " + ending;
        }

        public static string GetNotExistingFilePath(string destFilePath)
        {
            string destFileName = Path.GetFileNameWithoutExtension(destFilePath);
            string destFileExtension = Path.GetExtension(destFilePath);
            string destFolderPath = Path.GetDirectoryName(destFilePath);
            string newFileName = destFilePath;

            for (int i = 2; File.Exists(newFileName ); i++)
            {
                string fileName = $"{destFileName}({i}){destFileExtension}";
                newFileName = Path.Combine(destFolderPath, fileName);
            }

            return newFileName;
        }
    }
}