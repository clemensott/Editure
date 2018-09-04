using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MainProgram
{
    /// <summary>
    /// Interaktionslogik für AskCopyMove.xaml
    /// </summary>
    public partial class AskCopyMove : Window
    {
        private static Queue<Tuple<FileInfo, FileInfo, bool>> filePairs = new Queue<Tuple<FileInfo, FileInfo, bool>>();
        private static AskCopyMove instance;

        private AskCopyMove()
        {
            InitializeComponent();

            Application.Current.Exit += Application_Exit;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Close();
        }

        public static void Add(FileInfo Src, FileInfo wannaDest, bool copy)
        {
            lock (filePairs)
            {
                filePairs.Enqueue(new Tuple<FileInfo, FileInfo, bool>(Src, wannaDest, copy));
            }

            AskCopyMove askWindow = GetInstance();
            Action preparer = new Action(askWindow.PrepareWindowAndShow);
            Application.Current.Dispatcher.BeginInvoke(preparer);
        }

        private void PrepareWindowAndShow()
        {
            tblFilesCount.Text = "(" + filePairs.Count + ")";

            if (IsVisible) return;

            Prepare();
            ShowDialog();
        }

        private void Prepare()
        {
            if (filePairs.Count == 0) return;

            try
            {
                FileInfo currentSrc = filePairs.First().Item1;
                FileInfo currentWannaDest = filePairs.First().Item2;

                tblSrcSize.Text = GetFileInfoSize(currentSrc);
                tblSrcPath.Text = currentSrc.FullName;
                tblDestSize.Text = GetFileInfoSize(currentWannaDest);
                tblDestPath.Text = currentWannaDest.FullName;
            }
            catch { }
        }

        private static string GetFileInfoSize(FileInfo file)
        {
            string[] endings = new string[] { "B", "kB", "MB", "GB", "TB", "PB", "EB" };
            double value = Convert.ToDouble(file.Length);

            for (int i = 0; i < endings.Length; i++)
            {
                value = value / 1024.0;

                if (value < 1024) return Math.Round(value, 1) + " " + endings[i];
            }

            return Math.Round(value, 1).ToString() + " " + endings.Last();
        }

        private static AskCopyMove GetInstance()
        {
            if (instance == null) instance = new AskCopyMove();

            return instance;
        }

        private void BtnReplace_Click(object sender, RoutedEventArgs e)
        {
            lock (filePairs)
            {
                do
                {
                    ReplaceFiles();

                    if (filePairs.Count == 0)
                    {
                        Hide();
                        return;
                    }

                } while (cbxDoForAll.IsChecked == true);

            }

            Prepare();
        }

        private void BtnSkip_Click(object sender, RoutedEventArgs e)
        {
            lock (filePairs)
            {
                do
                {
                    filePairs.Dequeue();

                    if (filePairs.Count == 0)
                    {
                        Hide();
                        return;
                    }

                } while (cbxDoForAll.IsChecked == true);
            }

            Prepare();
        }

        private void BtnKeepBoth_Click(object sender, RoutedEventArgs e)
        {
            lock (filePairs)
            {
                do
                {
                    KeepBothFiles();

                    if (filePairs.Count == 0)
                    {
                        Hide();
                        return;
                    }

                } while (cbxDoForAll.IsChecked == true);
            }

            Prepare();
        }

        private void ReplaceFiles()
        {
            Tuple<FileInfo, FileInfo, bool> currentFilePair = filePairs.Dequeue();

            FileInfo currentSrc = currentFilePair.Item1;
            FileInfo currentWannaDest = currentFilePair.Item2;
            bool copy = currentFilePair.Item3;

            CopyMove(currentSrc, currentWannaDest, copy);
        }

        private void KeepBothFiles()
        {
            Tuple<FileInfo, FileInfo, bool> currentFilePair = filePairs.Dequeue();

            FileInfo currentSrc = currentFilePair.Item1;
            FileInfo Dest = GetNotExistingFileInfo(currentFilePair.Item2);
            bool copy = currentFilePair.Item3;

            CopyMove(currentSrc, Dest, copy);
        }

        private FileInfo GetNotExistingFileInfo(FileInfo file)
        {
            FileInfo fileInfo2 = new FileInfo(file.FullName);
            string fileNameWithoutExtension = GetFileNameWithoutExtention(file);

            for (int i = 2; fileInfo2.Exists; i++)
            {
                string fileName = string.Format("{0}({1}){2}", fileNameWithoutExtension, i, file.Extension);
                string path = Path.Combine(file.DirectoryName, fileName);

                fileInfo2 = new FileInfo(path);
            }

            return fileInfo2;
        }

        private string GetFileNameWithoutExtention(FileInfo fileInfo)
        {
            if (fileInfo.Extension.Length == 0) return fileInfo.Name;

            return fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length);
        }

        private void CopyMove(FileInfo Src, FileInfo wannaDest, bool copy)
        {
            try
            {
                wannaDest.Delete();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            try
            {
                if (copy) Src.CopyTo(wannaDest.FullName);
                else Src.MoveTo(wannaDest.FullName);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
    }
}
