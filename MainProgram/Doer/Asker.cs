using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MainProgram
{
    class Asker
    {
        private static readonly object lockObj = new object();
        private static Asker instance;

        public static Asker Current
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null) instance = new Asker();

                    return instance;
                }
            }
        }

        private Queue<Tuple<FileInfo, FileInfo>> queue;
        private Task mainTask;
        private bool applicationExited;

        private Asker()
        {
            queue = new Queue<Tuple<FileInfo, FileInfo>>();
            mainTask = Task.Factory.StartNew(new Action(Main));
            applicationExited = false;

            Application.Current.Exit += Application_Exit;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            applicationExited = true;
        }

        private void Main()
        {
            while (true)
            {
                Tuple<FileInfo, FileInfo> pair = Get();

                if (applicationExited) return;

                FileInfo Src = pair.Item1;
                FileInfo Dest = pair.Item2;
                string format = "Bild \"{0}\" ({1}) ersetzten durch Bild \"{2}\" ({3})?\nNein = Beide behalten";
                string text = string.Format(format, Dest.FullName,
                    GetFileInfoSize(Dest), Src.FullName, GetFileInfoSize(Src));

                MessageBoxResult result = MessageBox.Show(text, "Ersetzen", MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Yes) Copy(Src, Dest);
                else if (result == MessageBoxResult.No)
                {
                    FileInfo Dest2 = GetNotExistingFileInfo(Dest);

                    Copy(Src, Dest2);
                }
            }
        }

        public void Add(FileInfo Src, FileInfo wannaDest)
        {
            lock (queue)
            {
                queue.Enqueue(new Tuple<FileInfo, FileInfo>(Src, wannaDest));

                Monitor.Pulse(queue);
            }
        }

        private Tuple<FileInfo, FileInfo> Get()
        {
            lock(queue)
            {
                if (queue.Count == 0) Monitor.Wait(queue);
                if (applicationExited) return null;

                return queue.Dequeue();
            }
        }

        private void Copy(FileInfo Src, FileInfo Dest)
        {
            try
            {
                if (Dest.Exists) Dest.Delete();

                Src.CopyTo(Dest.FullName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private FileInfo GetNotExistingFileInfo(FileInfo file)
        {
            FileInfo fileInfo2 = new FileInfo(file.FullName);

            for (int i = 2; fileInfo2.Exists; i++)
            {
                string fileName = string.Format("{0}({1}){2}", GetFileNameWithoutExtention(file), i, file.Extension);
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

        private string GetFileInfoSize(FileInfo file)
        {
            string[] endings = new string[] { "B", "kB", "MB", "GB", "TB", "PB", "EB" };
            double value = Convert.ToDouble(file.Length);

            for (int i = 0; i < endings.Length; i++)
            {
                value = value / 1024.0;

                if (value < 1024) return value + " " + endings[i];
            }

            return Math.Round(value, 3).ToString() + " " + endings.Last();
        }
    }
}
