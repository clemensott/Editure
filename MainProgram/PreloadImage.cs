using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MainProgram
{
    class PreloadImage
    {
        private IList<FileInfo> pictures;
        private Tuple<string, BitmapImage> currentTuple, previousTuple, nextTuple;

        public BitmapImage CurrentImg { get { return currentTuple?.Item2; } }

        public string CurrentPath { get { return currentTuple?.Item1; } }

        public BitmapImage PreviousImg { get { return previousTuple?.Item2; } }

        public string PreviousPath { get { return previousTuple?.Item1; } }

        public BitmapImage NextImg { get { return nextTuple?.Item2; } }

        public string NextPath { get { return nextTuple?.Item1; } }

        public PreloadImage(IList<FileInfo> pictures)
        {
            this.pictures = pictures;

            Clear();
        }

        public async Task<BitmapImage> TryGetImageAsync(string path)
        {
            if (path == CurrentPath) return CurrentImg;
            if (path == PreviousPath) return PreviousImg;
            if (path == NextPath) return NextImg;

            try
            {
                return await EditImage.LoadBitmapAsync(path);
            }
            catch { }

            return null;
        }

        public void SetShowImage(string path, BitmapImage img)
        {
            if (path == CurrentPath) return;

            if (path == PreviousPath)
            {
                nextTuple = currentTuple;
                currentTuple = previousTuple;
                previousTuple = null;
            }

            if (path == NextPath)
            {
                previousTuple = currentTuple;
                currentTuple = nextTuple;
                nextTuple = null;
            }

            currentTuple = new Tuple<string, BitmapImage>(path, img);
            previousTuple = null;
            nextTuple = null;
        }

        public async Task UpdateOtherImages()
        {
            int showIndex = GetIndexOf(CurrentPath);

            if (showIndex == -1) return;

            if (nextTuple == null) nextTuple = await GetTupleAsync(showIndex, 1);
            if (previousTuple == null) previousTuple = await GetTupleAsync(showIndex, -1);
        }

        private async Task<Tuple<string, BitmapImage>> GetTupleAsync(int index, int offset)
        {
            int startIndex = index;

            if (pictures.Count == 0) return null;

            index = (index + offset + pictures.Count) % pictures.Count;

            try
            {
                string path = pictures[index].FullName;
                BitmapImage bmp = await EditImage.LoadBitmapAsync(path);

                return new Tuple<string, BitmapImage>(path, bmp);
            }
            catch { }

            return null;
        }

        private int GetIndexOf(string path)
        {
            int i = 0;

            foreach (FileInfo picture in pictures)
            {
                if (picture.FullName == path) return i;

                i++;
            }

            return -1;
        }

        public void Clear()
        {
            previousTuple = null;
            currentTuple = new Tuple<string, BitmapImage>("", null);
            nextTuple = null;
        }
    }
}
