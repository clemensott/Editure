using System.Windows.Media.Imaging;

namespace Editure.Backend.Editing.EditEncoders
{
    class EncoderManagerAuto : IEncoderManager
    {
        private string extension;

        public bool IsAuto => true;

        public bool IsBmp => false;

        public bool IsGif => false;

        public bool IsJpg => false;

        public bool IsPng => false;

        public bool IsTiff => false;

        public string Extension => extension;

        public BitmapEncoder Encoder { get; }

        public EncoderManagerAuto(string extension)
        {
            this.extension = extension;

            Encoder = GetEncoder();
        }

        public BitmapEncoder GetEncoder()
        {
            extension = extension.ToLower();

            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return new JpegBitmapEncoder();
                case ".png":
                    return new PngBitmapEncoder();
                case ".gif":
                    return new GifBitmapEncoder();
                case ".tiff":
                    return new TiffBitmapEncoder();
                default:
                    extension = ".bmp";

                    return new BmpBitmapEncoder();
            }
        }
    }
}
