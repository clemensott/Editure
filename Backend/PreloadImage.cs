using System;
using System.IO;
using System.Windows.Media.Imaging;
using Editure.Backend.Editing.PictureEditing;

namespace Editure.Backend
{
    class PreloadImage
    {
        private readonly CurrentItemList<FileInfo> pictures;
        private Tuple<string, BitmapImage> showImg, previousImg, nextImg;

        public BitmapImage Show => showImg.Item2;

        public string ShowPath => showImg.Item1;

        public BitmapImage Previous => previousImg.Item2;

        public string PreviousPath => previousImg.Item1;

        public BitmapImage Next => nextImg.Item2;

        public string NextPath => nextImg.Item1;

        public PreloadImage(CurrentItemList<FileInfo> pictures)
        {
            this.pictures = pictures;

            Clear();
        }

        public bool TrySetShowImage(string path)
        {
            if (path == showImg.Item1) return true;

            int showIndex = GetIndexOf(path);

            if (path == previousImg.Item1)
            {
                nextImg = showImg;
                showImg = previousImg;
                previousImg = GetTuple(showIndex, -1);

                return true;
            }

            if (path == nextImg.Item1)
            {
                previousImg = showImg;
                showImg = nextImg;
                nextImg = GetTuple(showIndex, 1);

                return true;
            }

            try
            {
                showImg = new Tuple<string, BitmapImage>(path, EditImage.LoadBitmap(path));
            }
            catch
            {
                return false;
            }

            previousImg = GetTuple(showIndex, -1);
            nextImg = GetTuple(showIndex, 1);

            return true;
        }

        private Tuple<string, BitmapImage> GetTuple(int index, int offset)
        {
            int startIndex = index;

            do
            {
                if (pictures.Count == 0) break;

                index = (index + offset + pictures.Count) % pictures.Count;

                try
                {
                    string path = pictures[index].FullName;
                    BitmapImage bmp = EditImage.LoadBitmap(path);

                    return new Tuple<string, BitmapImage>(path, bmp);
                }
                catch { }

            } while (index != startIndex);

            return new Tuple<string, BitmapImage>("", null);
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
            previousImg = new Tuple<string, BitmapImage>("", null);
            showImg = new Tuple<string, BitmapImage>("", null);
            nextImg = new Tuple<string, BitmapImage>("", null);
        }
    }
}
