using FolderFile;
using System;
using System.IO;
using System.Windows;

namespace MainProgram
{
    static class Utils
    {
        public static readonly FileInfo DefaultFileInfo = GetDefaultFileInfo();

        private static FileInfo GetDefaultFileInfo()
        {
            try
            {
                return new FileInfo("Editure.ico");
            }
            catch { }

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
            if (folder.IsAllDelete && (folder.Dest?.Directory?.Exists ?? false))
            {
                TryCatchWithMsgBox(folder.Dest.DeleteContent, errorMessage);
            }
        }

        public static void CopyMoveFile(FileInfo Src, FileInfo Dest, bool copy)
        {
            try
            {
                if (Dest.Exists) AskCopyMove.Add(Src, Dest, copy);
                else
                {
                    DirectoryInfo directory = new DirectoryInfo(Dest.DirectoryName);

                    if (!directory.Exists) directory.Create();

                    if (copy) Src.CopyTo(Dest.FullName);
                    else Src.MoveTo(Dest.FullName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Utils.CopyMoveFile");
            }
        }
    }
}
