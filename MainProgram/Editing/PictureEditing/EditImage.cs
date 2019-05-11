using System;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Threading.Tasks;

namespace MainProgram
{
    public class EditImage
    {
        private byte[] picBytes;
        private bool isImgFlipX, isImgFlipY;
        private BitmapSource img;

        public EditPictureProperties Properties;

        public IntSize OriginalSize { get; private set; }

        public EditImage(string path, EditPictureProperties properties)
        {
            picBytes = File.ReadAllBytes(path);

            BitmapImage originalBmp = LoadBitmap(picBytes, IntSize.Empty);
            OriginalSize = new IntSize(originalBmp.PixelWidth, originalBmp.PixelHeight);

            Properties = properties;

            SetImage();
        }

        public EditImage(string path, IntSize wanna, EditMode modeType, bool flipX, bool flipY,
            IntPoint offset, EditReferencePositionType referencePositionType) : 
            this(File.ReadAllBytes(path), wanna, modeType, flipX, flipY, offset, referencePositionType)
        {
        }

        public EditImage(byte[] pictureBytes, IntSize wanna, EditMode modeType, bool flipX, bool flipY,
            IntPoint offset, EditReferencePositionType referencePositionType)
        {
            picBytes = pictureBytes;

            BitmapImage originalBmp = LoadBitmap(picBytes, IntSize.Empty);
            OriginalSize = new IntSize(originalBmp.PixelWidth, originalBmp.PixelHeight);

            Properties = new EditPictureProperties(flipX, flipY,
                wanna, offset, modeType, referencePositionType);

            SetImage();
        }

        public static BitmapImage LoadBitmap(string path)
        {
            return LoadBitmap(File.ReadAllBytes(path), IntSize.Empty);
        }

        public static async Task<BitmapImage> LoadBitmapAsync(string path)
        {
            byte[] bytes = await Task.Run(() => File.ReadAllBytes(path));

            return LoadBitmap(bytes, IntSize.Empty);
        }

        private static BitmapImage LoadBitmap(byte[] data, IntSize IntSize)
        {
            BitmapImage loadImg = null;
            MemoryStream mem = new MemoryStream(data);

            loadImg = new BitmapImage();
            loadImg.BeginInit();
            loadImg.CacheOption = BitmapCacheOption.Default;
            loadImg.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            loadImg.DecodePixelWidth = IntSize.Width;
            loadImg.DecodePixelHeight = IntSize.Height;
            loadImg.StreamSource = mem;
            loadImg.Rotation = Rotation.Rotate0;
            loadImg.EndInit();

            return loadImg;
        }

        private void FlipBitmapSourceX()
        {
            int bytePerPixel = img.Format.BitsPerPixel / 8;
            int stride = img.PixelWidth * bytePerPixel;
            byte[] pixelData = new byte[stride * img.PixelHeight];

            img.CopyPixels(pixelData, stride, 0);

            for (int i = 0; i < img.PixelWidth / 2; i++)
            {
                for (int j = 0; j < img.PixelHeight; j++)
                {
                    int SrcIndex1 = j * stride + i * bytePerPixel;
                    int destinationIndex1 = 0;
                    int SrcIndex2 = (j + 1) * stride - (i + 1) * bytePerPixel;
                    int destinationIndex2 = j * stride + i * bytePerPixel;
                    int SrcIndex3 = 0;
                    int destinationIndex3 = (j + 1) * stride - (i + 1) * bytePerPixel;

                    byte[] pixelCache = new byte[bytePerPixel];

                    Array.Copy(pixelData, SrcIndex1, pixelCache, destinationIndex1, bytePerPixel);
                    Array.Copy(pixelData, SrcIndex2, pixelData, destinationIndex2, bytePerPixel);
                    Array.Copy(pixelCache, SrcIndex3, pixelData, destinationIndex3, bytePerPixel);
                }
            }

            WriteableBitmap target = new WriteableBitmap(img.PixelWidth, img.PixelHeight, img.DpiX, img.DpiY, img.Format, null);
            target.WritePixels(new Int32Rect(0, 0, img.PixelWidth, img.PixelHeight), pixelData, stride, 0);

            img = target;
        }

        private void FlipBitmapSourceY()
        {
            int bytePerPixel = img.Format.BitsPerPixel / 8;
            int stride = img.PixelWidth * bytePerPixel;
            byte[] pixelData = new byte[stride * img.PixelHeight];

            img.CopyPixels(pixelData, stride, 0);

            for (int i = 0; i < img.PixelHeight / 2; i++)
            {
                int SrcIndex1 = i * stride;
                int destinationIndex1 = 0;
                int SrcIndex2 = pixelData.Length - (i + 1) * stride;
                int destinationIndex2 = i * stride;
                int SrcIndex3 = 0;
                int destinationIndex3 = pixelData.Length - (i + 1) * stride;

                byte[] cache = new byte[stride];

                Array.Copy(pixelData, SrcIndex1, cache, destinationIndex1, cache.Length);
                Array.Copy(pixelData, SrcIndex2, pixelData, destinationIndex2, cache.Length);
                Array.Copy(cache, SrcIndex3, pixelData, destinationIndex3, cache.Length);
            }

            WriteableBitmap target = new WriteableBitmap(img.PixelWidth, img.PixelHeight, img.DpiX, img.DpiY, img.Format, null);
            target.WritePixels(new Int32Rect(0, 0, img.PixelWidth, img.PixelHeight), pixelData, stride, 0);

            img = target;
        }

        private void SetImage()
        {
            img = LoadBitmap(picBytes, Properties.GetScale(OriginalSize));
            isImgFlipX = false;
            isImgFlipY = false;
        }

        public BitmapSource GetEditBitmap()
        {
            if (img.PixelWidth != Properties.GetScale(OriginalSize).Width || img.PixelHeight != Properties.GetScale(OriginalSize).Height)
            {
                SetImage();
            }

            if (isImgFlipX != Properties.FlipX) FlipBitmapSourceX();
            if (isImgFlipY != Properties.FlipY) FlipBitmapSourceY();

            isImgFlipX = Properties.FlipX;
            isImgFlipY = Properties.FlipY;

            return GetCoppedImageSource();
        }

        private BitmapSource GetCoppedImageSource()
        {
            return new CroppedBitmap(BitmapFrame.Create(img), Properties.GetCrop(OriginalSize));
        }
    }
}
