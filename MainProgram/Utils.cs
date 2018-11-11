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
                ShowMessageBox(e, name);
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
                ShowMessageBox(e, name);
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
                ShowMessageBox(e, "Needed.CopyMoveFile");
            }
        }

        public static void ShowMessageBox(Exception e, string name)
        {
            string message = string.Empty;

            do
            {
                message += string.Format("{0}:\n{1}", e.GetType().Name, e.Message);

                if (e.InnerException == null) break;

                message += "\n\n";
                e = e.InnerException;
            } while (true);

            MessageBox.Show(message, name);
        }

        public static string GetMessage(Exception e)
        {
            if (e == null) return "Exception is null";

            string message = string.Empty;

            while (true)
            {
                message += string.Format("{0}:\n{1}", e.GetType().Name, e.Message);

                if (e.InnerException == null) break;

                message += "\n\n";
                e = e.InnerException;
            }

            return message;
        }
    }
}
